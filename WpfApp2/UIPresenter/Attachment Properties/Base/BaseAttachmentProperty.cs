using System;
using System.Windows;
using System.Windows.Controls;

namespace UI.UIPresenter.Attachment_Properties
{
    /// <summary>
    /// A base class for attached property
    /// </summary>
    /// <typeparam name="Parent">Parent class to be attached property</typeparam>
    /// <typeparam name="PropertyType">Type of this property</typeparam>
    public abstract class BaseAttachedProperty<Parent, PropertyType> where Parent : BaseAttachedProperty<Parent, PropertyType>, new()
    {

        #region Public Events

        /// <summary>
        /// Occures on Value of <see cref="ValueProperty"/> vhanged
        /// </summary>
        public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (sender, args) => {};

        #endregion

        #region Public Properties

        /// <summary>
        /// Singelton instance of our parent class
        /// </summary>
        public static Parent Instance { get; private set; } = new Parent();

        #endregion

        #region Attached Property Defenition

        /// <summary>
        /// The attached property for this class
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("Value", typeof(PropertyType), typeof(BaseAttachedProperty<Parent, PropertyType>), new PropertyMetadata(new PropertyChangedCallback(OnValuePropertyChanged)));

        /// <summary>
        /// The Collback event when the <see cref="ValueProperty"/> is changed
        /// </summary>
        /// <param name="d">The UI element that have it's property changed</param>
        /// <param name="e">Arguments for the event</param>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Call the perent function
            Instance.OnValueChanged(d, e);

            //Call event listeners
            Instance.ValueChanged(d, e);
        }

        /// <summary>
        /// Gets value of <see cref="ValueProperty"/>
        /// </summary>
        /// <param name="element">The element to get the property from</param>
        /// <returns></returns>
        public static PropertyType GetValue(DependencyObject element) => (PropertyType)element.GetValue(ValueProperty);

        /// <summary>
        /// Sets value to <see cref="ValueProperty"/>
        /// </summary>
        /// <param name="element">The element to set the value to</param>
        /// <param name="value">value to set</param>
        public static void SetValue(DependencyObject element, PropertyType value) => element.SetValue(ValueProperty, value);

        #endregion

        #region Event Methods

        /// <summary>
        /// The methods that is called when any attached property ov this type is changed
        /// </summary>
        /// <param name="d">The UI element which property is changed</param>
        /// <param name="e">Event args</param>
        public virtual void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e){}

        #endregion
    }
}
