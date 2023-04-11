﻿using InitialProject.Model;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository.Interfaces
{
    public interface ITourRepository : ISubject
    {
        public int NextId();
        public Tour Save(Tour tour);
        public Tour Update(Tour tour);
        public Tour GetById(int id);
        public List<Tour> GetAll();
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();

    }
}
