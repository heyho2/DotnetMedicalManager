using System;
using AutoMapper;
using GD.Common.Base;

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