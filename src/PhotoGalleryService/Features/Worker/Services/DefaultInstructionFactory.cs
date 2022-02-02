using Autofac;
using Autofac.Core;
using Boilerplate.Features.Core.Extensions;
using PhotoGalleryService.Features.Worker.Exceptions;
using PhotoGalleryService.Features.Worker.Instructions;
using System.Globalization;

namespace PhotoGalleryService.Features.Worker.Services
{
    public class DefaultInstructionFactory
        : IInstructionFactory
    {
        private readonly char _instructionsSeperator;
        private readonly char _parameterSeperator;

        private readonly IInstructionRegistry _registry;
        private readonly ILifetimeScope _scope;

        public DefaultInstructionFactory()
            : this(null, null, ';', ',')
        {
        }

        public DefaultInstructionFactory(IInstructionRegistry registry, ILifetimeScope scope)
            : this(registry, scope, ';', ',')
        {
        }

        public DefaultInstructionFactory(
            IInstructionRegistry registry,
            ILifetimeScope scope,
            char instructionSeperator, 
            char parameterSeperator)
        {
            _registry = registry;
            _scope = scope;
            _instructionsSeperator = instructionSeperator;
            _parameterSeperator = parameterSeperator;
        }

        public IEnumerable<IInstruction> Create(string instructions) 
        {
            List<IInstruction> actions = new List<IInstruction>();

            if(string.IsNullOrEmpty(instructions))
            {
                return actions;
            }

            foreach(string instruction in instructions.Split(_instructionsSeperator)) 
            {
                var name = GetName(instruction);
                var type = _registry.GetInstruction(name);

                if(type == null) 
                {
                    throw new InstructionNotFoundException($"Could not find instruction with name {name}");
                }

                actions.Add(Create(instruction, type));
            }

            return actions;
        }

        public string GetName(string instruction)
        {
            if (instruction.Contains('('))
            {
                return instruction.Split('(')[0];
            }

            return instruction;
        }

        private IInstruction Create(string instruction, Type type)
        {
            List<Parameter> parameters = new List<Parameter>();
            
            foreach(var p in GetParameters(instruction)) 
            {
                parameters.Add(new PositionalParameter(parameters.Count, p));
            }

            return (IInstruction)_scope.Resolve(type, parameters);
        }

        public IEnumerable<object> GetParameters(string instruction)
        {
            List<object> list = new List<object>();

            if(instruction.Contains('(')) 
            {
                string parameters = instruction.Split('(')[1].Split(')')[0];

                foreach(string parameter in parameters.Split(_parameterSeperator)) 
                {
                    if(string.IsNullOrEmpty(parameter))
                    {
                        continue;
                    }

                    list.Add(Parse(parameter));
                }
            }

            return list;
        }

        public object Parse(string parameter) 
        {
            if(IsNumeric(parameter))
            {
                if(IsDouble(parameter))
                {
                    return parameter.GetAs<double>();
                }

                return parameter.GetAs<int>();
            }
            else if(IsBoolean(parameter)) 
            {
                return parameter.GetAs<bool>();
            }

            return parameter;
        }

        private bool IsNumeric(string value) 
        {
            value = value.Replace(".", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            value = value.Replace(",", CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

            return double.TryParse(value, out double _);
        }

        private bool IsDouble(string value) 
        {
            return value.Contains('.');
        }

        private bool IsBoolean(string value) 
        {
            if(string.IsNullOrEmpty(value))
            {
                return false;
            }

            return value.Equals("true", StringComparison.InvariantCultureIgnoreCase) 
                || value.Equals("false", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
