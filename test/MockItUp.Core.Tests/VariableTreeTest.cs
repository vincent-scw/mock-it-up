using MockItUp.Core.PlaceHolder;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MockItUp.Core.Tests
{
    public class VariableTreeTest
    {
        [Fact]
        public void BuildVariableTree_RetreiveData_ShouldAsExpected()
        {
            var list = new Dictionary<string, string>
            {
                { "b.order.id", "100" },
                { "b.shipment.id", "S001" },
                { "b.order.itemsCount", "5" },
                { "p.id", "100" },
                { "header.accepts", "application/json" }
            };

            var tree = new VariableTree<string>();
            foreach (var item in list)
            {
                tree.AppendNode(item.Key, item.Value);
            }

            Assert.Equal(3, tree.Root.Children.Count);

            var found = tree.TryGet("body.order.id", out string value);
            Assert.True(found);
            Assert.Equal("100", value);

            found = tree.TryGet("b.order", out value);
            Assert.False(found);
            Assert.Null(value);

            found = tree.TryGet("abc", out value);
            Assert.False(found);
            Assert.Null(value);
        }
    }
}
