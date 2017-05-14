using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AD.PartialEquilibriumApi.Xml;
using JetBrains.Annotations;

namespace AD.PartialEquilibriumApi
{
    public class Market
    {
        public double ElasticityOfSubstitution { get; set; }
        
        public double ElasticityOfSupply { get; set; }

        public double ElasticityOfDemand { get; set; }
        
        public double InitialPrice { get; set; }

        public double ProducerPrice => InitialPrice / (1 + Shock);
        
        public double InitialMarketShare { get; set; }

        public double Shock { get; set; }

        public double MarketShare { get; set; }

        public Market[] UpstreamMarkets { get; set; } = new Market[0];

        public Market[] DownstreamMarkets { get; set; } = new Market[0];

        public double ConsumerPrice { get; set; }

        public double MarketEquilibrium { get; set; }

        public double SumOfSquaresMinimizer =>
            MarketEquilibrium * MarketEquilibrium + UpstreamMarkets.Sum(x => x.SumOfSquaresMinimizer);

        public void SetConsumerPrice(double[] values)
        {
            for (int i = 0; i < UpstreamMarkets.Length; i++)
            {
                UpstreamMarkets[i].SetConsumerPrice(values.Skip(i).ToArray());
            }

            double consumerPriceIndexComponents =
                UpstreamMarkets.Any()
                    ? UpstreamMarkets.Sum(x => x.InitialMarketShare * Math.Pow(x.InitialPrice, 1 - x.ElasticityOfSubstitution))
                    : InitialMarketShare * Math.Pow(InitialPrice, 1 - ElasticityOfSubstitution);

            double consumerPriceIndex =
                Math.Pow(consumerPriceIndexComponents, 1 / (1 - ElasticityOfSubstitution));

            double consumerPrice = 
                consumerPriceIndex + values.First();
                //UpstreamMarkets.Any() ? /*values[--index] **/ consumerPriceIndex : values.First();

            ConsumerPrice = consumerPrice;
        }

        /// <summary>
        /// Calculates market shares on this and descendant <see cref="XElement"/> objects in reverse document order.
        /// Result = [γ_i * price_i^(1-σ_i)] / Σ_j [γ_j * price_j^(1-σ_j)].
        /// </summary>
        /// <param name="model">An <see cref="XElement"/> describing the model.</param>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public void CalculateMarketShares()
        {
            foreach (Market market in UpstreamMarkets)
            {
                market.CalculateMarketShares();
            }

            double expenditure =
                InitialMarketShare * Math.Pow(ConsumerPrice, 1 - ElasticityOfSubstitution);

            double totalExpenditure =
                DownstreamMarkets.Any()
                    ? DownstreamMarkets.Sum(x => x.UpstreamMarkets.Sum(y => y.InitialMarketShare * Math.Pow(y.ConsumerPrice, 1 - y.ElasticityOfSubstitution)))
                    : InitialMarketShare * Math.Pow(ConsumerPrice, 1 - ElasticityOfSubstitution);

            MarketShare =
                expenditure / totalExpenditure;
        }

        /// <summary>
        /// Sets the MarketEquilibrium attribute on descendant <see cref="XElement"/> objects in reverse document order.
        /// Result = (producerPrice ^ elasticityOfSupply) - [(consumerConsumerPriceIndex ^ (elasticityOfSubstitution + elasticityOfDemand)) / (consumerPrice ^ elasticityOfSubstitution)]
        /// </summary>
        /// <returns>A reference to the existing <see cref="XElement"/>. This is returned for use with fluent syntax calls.</returns>
        public void CalculateMarketEquilibrium()
        {
            foreach (Market market in UpstreamMarkets)
            {
                market.CalculateMarketEquilibrium();
            }

            double priceIndexComponents =
                    MarketShare * Math.Pow(ConsumerPrice, 1 - ElasticityOfSubstitution);

            double priceIndex =
                Math.Pow(priceIndexComponents, 1 / (1 - ElasticityOfSubstitution));
                
            double marketEquilibrium =
                Math.Pow(ProducerPrice, ElasticityOfSupply)
                -
                Math.Pow(priceIndex, ElasticityOfSubstitution + ElasticityOfDemand)
                /
                Math.Pow(ConsumerPrice, ElasticityOfSubstitution);

            MarketEquilibrium = marketEquilibrium;
        }
    }
}
