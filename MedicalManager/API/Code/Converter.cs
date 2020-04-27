using AutoMapper;
using GD.Common.Base;
using GD.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GD.API.Code
{
    /// <summary>
    /// 
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Dto2Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static TModel ToModel<TModel>(this BaseDto dto) where TModel : BaseModel
        {
            if (dto == null)
            {
                throw new NullReferenceException();
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap(dto.GetType(), typeof(TModel)));
            var mapper = config.CreateMapper();
            return mapper.Map<TModel>(dto);
        }

        /// <summary>
        /// Model2Dto
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TDto ToDto<TDto>(this BaseModel model) where TDto : BaseDto
        {
            if (model == null)
            {
                throw new NullReferenceException();
            }

            var config = new MapperConfiguration(cfg => cfg.CreateMap(model.GetType(), typeof(TDto)));
            var mapper = config.CreateMapper();
            return mapper.Map<TDto>(model);
        }
        /// <summary>
        /// tree
        /// </summary>
        /// <typeparam name="ResultT"></typeparam>
        /// <typeparam name="ModelT"></typeparam>
        /// <param name="models"></param>
        /// <param name="pid"></param>
        /// <param name="pidFunc"></param>
        /// <param name="idFunc"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public static IList<ResultT> GetTree<ResultT, ModelT>(this IEnumerable<ModelT> models, string pid,
            Func<ModelT, string> pidFunc, Func<ModelT, string> idFunc, Func<ModelT, ResultT> select) where ResultT : BaseTreeDto<ResultT>, new() where ModelT : class
        {
            IEnumerable<ModelT> _models;
            if (pid == null)
            {
                _models = models.Where(a => pidFunc(a) == pid || !models.Any(b => idFunc(b) == pidFunc(a)));
            }
            else
            {
                _models = models.Where(a => pidFunc(a) == pid);
            }
            if (_models.Count() == 0)
            {
                return new List<ResultT>();
            }
            var tree = _models.Select(a =>
            {
                ResultT result = select(a);
                result.Children = models.GetTree(idFunc(a), pidFunc, idFunc, select);
                return result;
            }).ToList();
            return tree;
        }
        /// <summary>
        /// 获取结构所有结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aa"></param>
        /// <returns></returns>
        public static IList<string> GetValues<T>(T aa) where T : struct
        {
            FieldInfo[] fileds = typeof(T).GetFields();
            return fileds.Select(a => a.GetValue(aa)?.ToString()).ToList();
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string stringValue) where TEnum : struct
        {
            var res = Enum.TryParse(stringValue, out TEnum result);
            return result;
        }

        /// <summary>
        ///  类型映射,默认字段名字一一对应
        /// </summary>
        /// <typeparam name="TDestination">转化之后的model，可以理解为viewmodel</typeparam>
        /// <typeparam name="TSource">要被转化的实体，Entity</typeparam>
        /// <param name="source">可以使用这个扩展方法的类型，任何引用类型</param>
        /// <returns>转化之后的实体</returns>
        public static TDestination MapTo<TDestination, TSource>(this TSource source)
            where TDestination : BaseDto
            where TSource : BaseDto
        {
            if (source == null) return default(TDestination);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }
    }
}
