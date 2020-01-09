using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Personal_App.Domain.CustomDependencyProperties
{
    public class MaxByteAttachedProperty : DependencyObject
    {
        public enum Encode
        {
            Default,
            ASCII,
            UTF8,
            UTF32,
            UTF7,
            BigEndianUnicode,
            Unicode
        }


        private static string GetPreText(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(PreTextProperty);
        }

        private static void SetPreText(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(PreTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for PreText. This enables animation, styling, binding, etc...
        private static readonly DependencyProperty PreTextProperty =
         DependencyProperty.RegisterAttached("PreText", typeof(string), typeof(MaxByteAttachedProperty), new PropertyMetadata(""));


        public static int GetMaxByteLength(DependencyObject dependencyObject)
        {
            return (int)dependencyObject.GetValue(MaxByteLengthProperty);
        }

        public static void SetMaxByteLength(DependencyObject dependencyObject, int value)
        {
            dependencyObject.SetValue(MaxByteLengthProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for MaxByteLength. This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MaxByteLengthProperty =
         DependencyProperty.RegisterAttached("MaxByteLength", typeof(int), typeof(MaxByteAttachedProperty), new PropertyMetadata(OnTextBoxPropertyChanged));

        private static void OnTextBoxPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = dependencyObject as TextBox;
            if (textBox == null)
            {
                return;
            }
            textBox.TextChanged += TextBox_TextChanged;
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (IsOutMaxByteLength(textBox.Text, textBox))
            {
                textBox.Text = GetPreText(textBox);
                textBox.Select(textBox.Text.Length, 0);
                return;
            }
        }

        public static Encode GetEncodeModel(DependencyObject dependencyObject)
        {
            return (Encode)dependencyObject.GetValue(EncodeModelProperty);
        }

        public static void SetEncodeModel(DependencyObject obj, Encode value)
        {
            obj.SetValue(EncodeModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for EncodeM. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EncodeModelProperty =
         DependencyProperty.RegisterAttached("EncodeModel", typeof(Encode), typeof(MaxByteAttachedProperty), new PropertyMetadata(Encode.UTF8, OnEncodeModelChanged));
        private static void OnEncodeModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetEM(d, GetEncodeModel(d));
        }

        private static Encoding GetEncodingModel(DependencyObject obj)
        {
            return (Encoding)obj.GetValue(EncodingModelProperty);
        }

        private static void SetEncodingModel(DependencyObject obj, Encoding value)
        {
            obj.SetValue(EncodingModelProperty, value);
        }

        // Using a DependencyProperty as the backing store for EncodingModel. This enables animation, styling, binding, etc...
        private static readonly DependencyProperty EncodingModelProperty =
         DependencyProperty.RegisterAttached("EncodingModel", typeof(Encoding), typeof(MaxByteAttachedProperty), new PropertyMetadata(Encoding.UTF8));

        private static void SetEM(DependencyObject obj, Encode e)
        {
            switch (e)
            {
                case Encode.Default:
                    SetEncodingModel(obj, Encoding.Default);
                    break;
                case Encode.ASCII:
                    SetEncodingModel(obj, Encoding.ASCII);
                    break;
                case Encode.UTF8:
                    SetEncodingModel(obj, Encoding.UTF8);
                    break;
                case Encode.UTF32:
                    SetEncodingModel(obj, Encoding.UTF32);
                    break;
                case Encode.UTF7:
                    SetEncodingModel(obj, Encoding.UTF7);
                    break;
                case Encode.BigEndianUnicode:
                    SetEncodingModel(obj, Encoding.BigEndianUnicode);
                    break;
                case Encode.Unicode:
                    SetEncodingModel(obj, Encoding.Unicode);
                    break;
                default:
                    break;
            }
        }

        private static bool IsOutMaxByteLength(string txt, DependencyObject obj)
        {
            int txtLength = GetEncodingModel(obj).GetBytes(txt).Length;//文本长度
            if (GetMaxByteLength(obj) >= txtLength)
            {
                SetPreText(obj, txt);
                return false;
            }
            return true;
        }
    }
}
