using System;
using System.Linq.Expressions;

using NET.Repository.Logic;
using NET.Repository.Tests.Fakes.DTO;

namespace NET.Repository.Tests.Fakes.Specifications
{
    public class CartoonIdEqualitySpecification : Specification<Cartoon>
    {
        protected readonly short _id;
        protected readonly bool _onDefault;
        public CartoonIdEqualitySpecification(short id, bool onDefault = false)
        {
            _id = id;
            _onDefault = onDefault;
        }

        public override Expression<Func<Cartoon, bool>> ToExpression()
            => cartoon => _id == default ? _onDefault : cartoon.Id == _id;
    }
}