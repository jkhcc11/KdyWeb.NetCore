using System;
using System.ComponentModel.DataAnnotations;

namespace KdyWeb.BaseInterface
{
    /// <summary>
    /// Ef查询扩展
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class KdyQueryAttribute : Attribute
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="field">
        ///  字段 多个表可以直接用.
        /// <example>
        ///    eg:Name <br/>
        ///      Student.Name <br/>
        /// </example>
        /// </param>
        /// <param name="operator"></param>
        public KdyQueryAttribute(string field, KdyOperator @operator)
        {
            Field = field;
            Operator = @operator;
        }

        /// <summary>
        /// 实体类字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 条件运算符
        /// </summary>
        public KdyOperator Operator { get; set; }

    }

    /// <summary>
    /// 运算符
    /// </summary>
    public enum KdyOperator
    {
        /// <summary>
        /// Like
        /// </summary>
        Like,

        /// <summary>
        /// Not Like
        /// </summary>
        NotLike,

        /// <summary>
        /// 包含
        /// </summary>
        Include,

        /// <summary>
        /// 不包含
        /// </summary>
        NotInclude,

        /// <summary>
        /// 大于
        /// </summary>
        [Display(Name = ">")]
        Gt,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Display(Name = ">=")]
        GtEqual,

        /// <summary>
        /// 小于
        /// </summary>
        [Display(Name = "<")]
        Less,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Display(Name = "<=")]
        LessEqual,

        /// <summary>
        /// 等于
        /// </summary>
        [Display(Name = "=")]
        Equal,

        /// <summary>
        /// 不等于
        /// </summary>
        [Display(Name = "!=")]
        NotEqual,

        /// <summary>
        /// 字符开始
        /// </summary>
        StartsWith,

        /// <summary>
        /// 非字符开始
        /// </summary>
        NotStartsWith,

        /// <summary>
        /// 字符结束
        /// </summary>
        EndsWith,

        /// <summary>
        /// 非字符结束
        /// </summary>
        NotEndsWith,
    }
}
