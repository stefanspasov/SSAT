﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestEnvironment.Entities;
using TestEnvironment.Organizers;

namespace TestEnvironment
{
    public class Environment
    {
        private static Environment _instance;
        public static Environment Instance
        {
            get { if (_instance == null) _instance = new Environment(); return _instance; }
        }

        private Environment() { }

        public void Setup(TestCase testCase)
        {
            var clientTechnologyPairs = testCase.Steps.SelectMany(s => s.Actions).Select(a => new { a.TargetClient.IpAddress, TestTechnology = a.Operation.Executor }).Distinct().ToList();
            foreach (var pair in clientTechnologyPairs)
            {
                IOrganizer organizer = OrganizerFactory.Instance.CreateOrganizer(pair.TestTechnology);
                if (organizer != null)
                {
                    organizer.Setup(pair.IpAddress);
                }   
            }
        }
    }
}