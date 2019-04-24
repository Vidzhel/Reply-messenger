﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UI.UIPresenter.Converters
{

    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter where T: class, new()
    {

        private static T Converter = null;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Converter ?? (Converter = new T()); 
        }

        #region ValueConverters

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
