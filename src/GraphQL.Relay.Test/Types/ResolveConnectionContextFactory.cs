﻿using System.Collections.Generic;
using GraphQL.Builders;
using GraphQL.Execution;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GraphQL.Relay.Test.Types
{
    public class ResolveConnectionContextFactory
    {
        public static ResolveConnectionContext<TestParent> CreateContext(int? first = null, string after = null,
            int? last = null, string before = null)
        {
            return new ResolveConnectionContext<TestParent>(
                new ResolveFieldContext() { },
                true,
                null
            )
            {
                Arguments = new Dictionary<string, ArgumentValue>
                {
                    ["first"] = new ArgumentValue(first, ArgumentSource.Variable),
                    ["last"] = new ArgumentValue(last, ArgumentSource.Variable),
                    ["after"] = new ArgumentValue(after, ArgumentSource.Variable),
                    ["before"] = new ArgumentValue(before, ArgumentSource.Variable)
                }
            };
        }

        public class TestParent
        {
        }
    }
}
