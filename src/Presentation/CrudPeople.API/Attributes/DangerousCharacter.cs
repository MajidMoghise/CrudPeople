using System.ComponentModel.DataAnnotations;
using AspectInjector.Broker;



namespace CrudPeople.API.Attributes
{
    [Aspect(Scope.Global)]
    [Injection(typeof(DangerousCharacter))]
    [AttributeUsage(AttributeTargets.All)]
    public class DangerousCharacter : Attribute
    {
        readonly List<string> _characters = new List<string> { "'" };
        [Advice(Kind.Before)]
        public void Validate([Argument(Source.Arguments)] object[] objects)
        {
            foreach (var item in objects)
            {

                if (item is not null && item.GetType() == typeof(string))
                {
                    if (_characters.Any(w => item.ToString().Contains(w)))
                    {
                        throw new ValidationException("Bad Request, Request Consist of dangerous character");
                    }
                }
            }
        }
    }
}
