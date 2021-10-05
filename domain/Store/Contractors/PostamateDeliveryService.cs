using Store;
using Store.Contractors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contractors.Postamate
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

        public string UniqueCode => "Postamate";

        public string Title => "Delivery by mail in Los Angeles and New York";

        public OrderDelivery GetDelivery(Form form)
        {
            //if (form == null)
            //    throw new ArgumentNullException(nameof(form));

            if (form.UniqueCode != UniqueCode || /*form.Step != 3 || */!form.IsFinal)
                throw new InvalidOperationException("Unrecognized form data.");

            //var cityId = form.Fields[0].Value;
            //var city = cities[cityId];
            //var postamateId = form.Fields[1].Value;
            //var postamate = postamates[cityId][postamateId];

            //var description = $"City: {city}\nPost: {postamate}\n";
            //var parameters = new { CityId = cityId, PostamateId = postamateId };

            //return new OrderDelivery(UniqueCode, description, parameters, 100m);

            var cityId = form.Fields.Single(field => field.Name == "city").Value;
            var cityName = cities[cityId];
            var postamateId = form.Fields.Single(field => field.Name == "postamate").Value;
            var postamateName = postamates[cityId][postamateId];    
            var parameters = new Dictionary<string, string>
            {
                {nameof(cityId), cityId },
                {nameof(cityName), cityName },
                {nameof(postamateId), postamateId },
                {nameof(postamateName),postamateName },
            };

            var description = $"City: {cityName}\nPost: {postamateName}\n";
            return new OrderDelivery(UniqueCode, description, parameters, 0m);
        }

        public Form CreateForm(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            return new Form(UniqueCode, order.Id, 1, false, new[]
            {
                new SelectionField("City", "city", "1", cities),
            });
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"] == "1")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("City", "city", "1"),
                        new SelectionField("Post", "postamate", "1", postamates["1"]),
                    });
                }
                else if (values["city"] == "2")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("City", "city", "2"),
                        new SelectionField("Post", "postamate", "4", postamates["2"]),
                    });
                }
                else
                    throw new InvalidOperationException("Invalid post city.");
            }
            else if (step == 2)
            {
                return new Form(UniqueCode, orderId, 3, true, new Field[]
                {
                    new HiddenField("City", "city", values["city"]),
                    new HiddenField("Post", "postamate", values["postamate"]),
                });
            }
            else
                throw new InvalidOperationException("Invalid postamate step.");
        }
    }
}
