using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ST.Common.Domain
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }


    public class FeedBacktextValidationRule : ValidationRule
    {
        public int Minimum { get; set; } = 5;
        public int Maximum { get; set; } = 1000;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((value ?? "").ToString()))
            {
                return new ValidationResult(false, "请尽量详细描述问题（需不少于5字）.");
            }

            if (value.ToString().Length < Minimum || value.ToString().Length > Maximum)
            {
                return new ValidationResult(false, "请尽量详细描述问题（需不少于5字）.");
            }

            return ValidationResult.ValidResult;
        }
    }

    public class EmptyPhoneNumValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "请输入手机号码.")
                : ValidationResult.ValidResult;
        }
    }

    public class ErrPhoneNumValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "请输入手机号码.")
                : ValidationResult.ValidResult;
        }
    }

    public class EmptyPhoneCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "请输入手机验证码.")
                : ValidationResult.ValidResult;
        }
    }

    public class ErrPhoneCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "请输入手机验证码.")
                : ValidationResult.ValidResult;
        }
    }

    public class EmptyCheckCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "请输入图片验证码.")
                : ValidationResult.ValidResult;
        }
    }

    public class ErrCheckCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "请输入图片验证.")
                : ValidationResult.ValidResult;
        }
    }
}
