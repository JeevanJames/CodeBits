using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CodeBitsWP
{
    public sealed class AppBarHelper
    {
        private readonly IApplicationBar _applicationBar;
        private readonly AppBarGroups _groups = new AppBarGroups();

        public AppBarHelper(IApplicationBar applicationBar)
        {
            if (applicationBar == null)
                throw new ArgumentNullException("applicationBar");
            _applicationBar = applicationBar;
        }

        public void RegisterGroup(AppBarGroup group)
        {
            _groups.Add(group);
        }

        public void Update(object groupIdentifier)
        {
            if (groupIdentifier == null)
                throw new ArgumentNullException("groupIdentifier");

            _applicationBar.Buttons.Clear();
            _applicationBar.MenuItems.Clear();

            AppBarGroup matchingGroup = _groups.FirstOrDefault(group => group.GroupIdentifier.Equals(groupIdentifier));
            if (matchingGroup == null)
                throw new ArgumentException(
                    string.Format("Group identifier {0} could not be found in the registered groups", groupIdentifier), "groupIdentifier");

            bool hasNoItems = matchingGroup.IconButtons.Count == 0 && matchingGroup.MenuItems.Count == 0;
            _applicationBar.IsVisible = !hasNoItems;
            if (hasNoItems)
                return;

            bool hasOnlyMenuItems = matchingGroup.IconButtons.Count == 0;
            if (hasOnlyMenuItems && !matchingGroup.Mode.HasValue)
                _applicationBar.Mode = ApplicationBarMode.Minimized;
            else
                _applicationBar.Mode = matchingGroup.Mode.GetValueOrDefault(ApplicationBarMode.Default);

            foreach (AppBarIconButton iconButton in matchingGroup.IconButtons)
            {
                var button = new ApplicationBarIconButton(iconButton.RelativeUrl);
                button.Text = iconButton.Text;
                button.Click += iconButton.ClickHandler;
                _applicationBar.Buttons.Add(button);
            }

            if (matchingGroup.MenuItems.Count > 0)
            {
                _applicationBar.IsMenuEnabled = true;
                foreach (AppBarMenuItem menuItem in matchingGroup.MenuItems)
                {
                    var item = new ApplicationBarMenuItem(menuItem.Text);
                    item.Click += menuItem.ClickHandler;
                    _applicationBar.MenuItems.Add(item);
                }
            } else
                _applicationBar.IsMenuEnabled = false;

            _applicationBar.Opacity = matchingGroup.Opacity;
            if (matchingGroup.OnActivated != null)
                matchingGroup.OnActivated(matchingGroup.GroupIdentifier);
        }
    }

    public static class AppBarHelperExtensions
    {
        /// <summary>
        /// Wires up a Panorama control with an AppBarHelper, so that the application bar automatically
        /// updates whenever the current panorama item is changed.
        /// </summary>
        /// <param name="panorama">The Panorama control to wire up</param>
        /// <param name="helper">The AppBarHelper instance to use to wire up the Panorama control</param>
        public static void WireupAppBarHelper(this Panorama panorama, AppBarHelper helper)
        {
            if (panorama == null)
                throw new ArgumentNullException("panorama");
            if (helper == null)
                throw new ArgumentNullException("helper");

            panorama.SelectionChanged += (sender, args) => {
                if (args.AddedItems.Count > 0)
                    helper.Update(args.AddedItems[0]);
            };

            //Update the appbar for the initially-selected panorama item
            if (panorama.SelectedItem != null)
                helper.Update(panorama.SelectedItem);
        }

        /// <summary>
        /// Wires up a Pivot control with an AppBarHelper, so that the application bar automatically
        /// updates whenever the current pivot item is changed.
        /// </summary>
        /// <param name="pivot">The Pivot control to wire up</param>
        /// <param name="helper">The AppBarHelper instance to use to wire up the Pivot control</param>
        public static void WireupAppBarHelper(this Pivot pivot, AppBarHelper helper)
        {
            if (pivot == null)
                throw new ArgumentNullException("pivot");
            if (helper == null)
                throw new ArgumentNullException("helper");

            pivot.SelectionChanged += (sender, args) => {
                if (args.AddedItems.Count > 0)
                    helper.Update(args.AddedItems[0]);
            };

            //Update the appbar for the initially-selected pivot item
            if (pivot.SelectedItem != null)
                helper.Update(pivot.SelectedItem);
        }
    }

    /// <summary>
    /// Defines the properties for a representation of the application bar.
    /// </summary>
    public sealed class AppBarGroup
    {
        private readonly object _groupIdentifier;
        private readonly List<AppBarIconButton> _iconButtons = new List<AppBarIconButton>();
        private readonly List<AppBarMenuItem> _menuItems = new List<AppBarMenuItem>();

        public AppBarGroup(object groupIdentifier, IEnumerable<AppBarIconButton> iconButtons = null, IEnumerable<AppBarMenuItem> menuItems = null)
        {
            if (groupIdentifier == null)
                throw new ArgumentNullException("groupIdentifier");

            _groupIdentifier = groupIdentifier;
            if (iconButtons != null)
                _iconButtons.AddRange(iconButtons);
            if (menuItems != null)
                _menuItems.AddRange(menuItems);

            Opacity = 1.0d;
            Mode = null;
        }

        public object GroupIdentifier
        {
            get { return _groupIdentifier; }
        }

        public IList<AppBarIconButton> IconButtons
        {
            get { return _iconButtons; }
        }

        public IList<AppBarMenuItem> MenuItems
        {
            get { return _menuItems; }
        }

        public double Opacity { get; set; }

        /// <summary>
        /// The ApplicationBarMode of the application bar when this group is activated. This field
        /// is nullable; specifying null means that the AppBarHelper will use some built-in intelligent
        /// rules for deciding the application bar mode.
        /// </summary>
        public ApplicationBarMode? Mode { get; set; }

        /// <summary>
        /// Optional function that can be called when the group is activated to perform some custom
        /// actions.
        /// </summary>
        public Action<object> OnActivated { get; set; }
    }

    internal sealed class AppBarGroups : KeyedCollection<object, AppBarGroup>
    {
        protected override object GetKeyForItem(AppBarGroup item)
        {
            return item.GroupIdentifier;
        }
    }

    public sealed class AppBarIconButton
    {
        private readonly string _text;
        private readonly Uri _relativeUrl;
        private readonly EventHandler _clickHandler;

        public AppBarIconButton(string text, string relativeUrl, EventHandler clickHandler)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (relativeUrl == null)
                throw new ArgumentNullException("relativeUrl");
            if (clickHandler == null)
                throw new ArgumentNullException("clickHandler");

            _text = text;
            _relativeUrl = new Uri(relativeUrl, UriKind.Relative);
            _clickHandler = clickHandler;
        }

        public string Text
        {
            get { return _text; }
        }

        public Uri RelativeUrl
        {
            get { return _relativeUrl; }
        }

        public EventHandler ClickHandler
        {
            get { return _clickHandler; }
        }
    }

    public sealed class AppBarMenuItem
    {
        private readonly string _text;
        private readonly EventHandler _clickHandler;

        public AppBarMenuItem(string text, EventHandler clickHandler)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (clickHandler == null)
                throw new ArgumentNullException("clickHandler");
            _text = text;
            _clickHandler = clickHandler;
        }

        public string Text
        {
            get { return _text; }
        }

        public EventHandler ClickHandler
        {
            get { return _clickHandler; }
        }
    }
}