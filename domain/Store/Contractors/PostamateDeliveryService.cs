using Store;
using System;
using System.Collections.Generic;

namespace Store.Contractors
{
    public class PostamateDeliveryService : IDeliveryService
    {
        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            { "1", "Los Angeles" },
            { "2", "New York" },
        };

        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            {
                "1",
                new Dictionary<string, string>
                {
                    { "1", "Hollywood L.A." },
                    { "2", "Santa Monica" },
                    { "3", "Pasadena" },
                }
            },
            {
                "2",
                new Dictionary<string, string>
                {
                    { "4", "Manhattan" },
                    { "5", "Brooklyn" },
                    { "6", "Bronx" },
                }
            }
        };

        public string Name => "Postamate";

        public string Title => "Delivery by mail in Los Angeles and New York";

        public Form FirstForm(int orderId, IEnumerable<Book> books)
        {
            return Form.CreateFirst(Name)
                        .AddParameter(nameof(orderId), orderId.ToString())
                        .AddField(new SelectionField("City", "city", "1", cities));
        }
    

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"] == "1")
                {
                    return Form.CreateNext(Name, 2, values)
                              .AddField(new SelectionField("Post", "postamate", "1", postamates["1"]));
                }
                else if (values["city"] == "2")
                {
                    return Form.CreateNext(Name, 2, values)
                               .AddField(new SelectionField("Post", "postamate", "4", postamates["2"]));
                }
                else
                    throw new InvalidOperationException("Invalid post city.");
            }
            else if (step == 2)
            {
                return Form.CreateLast(Name, 3, values);
            }
            else
                throw new InvalidOperationException("Invalid postamate step.");
        }

        public OrderDelivery GetDelivery(Form form)
        {
            if (form.ServiceName != Name || !form.IsFinal)
                throw new InvalidOperationException("Invalid form.");

            var cityId = form.Parameters["city"];
            var cityName = cities[cityId];
            var postamateId = form.Parameters["postamate"];
            var postamateName = postamates[cityId][postamateId];

            var parameters = new Dictionary<string, string>
            {
                { nameof(cityId), cityId },
                { nameof(cityName), cityName },
                { nameof(postamateId), postamateId },
                { nameof(postamateName), postamateName },
            };

            var description = $"City: {cityName}\nPost: {postamateName}";

            return new OrderDelivery(Name, description, 150m, parameters);
        }
    }
}
