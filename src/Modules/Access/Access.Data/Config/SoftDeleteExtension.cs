﻿using Access.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Access.Data.Config
{
    public static class SoftDeleteExtension
    {
        public static void AddSoftDeleteQueryFilter(
        this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteExtension)?
                .GetMethod(nameof(GetSoftDeleteFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)?
                .MakeGenericMethod(entityData.ClrType)!;
            var filter = methodToCall?.Invoke(null, new object[] { })!;
            entityData.SetQueryFilter((LambdaExpression)filter);
            entityData.AddIndex(entityData.
                 FindProperty(nameof(IBaseEntity.IsDeleted))!);
        }
        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : IBaseEntity
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }

}
