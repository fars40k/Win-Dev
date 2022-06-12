﻿using System;

namespace MvvmLightTest.Model
{
    public class DataService : IDataService
    {
        public void GetPerson(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }
    }
}