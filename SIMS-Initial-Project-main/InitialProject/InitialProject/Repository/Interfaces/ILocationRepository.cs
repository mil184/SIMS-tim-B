﻿using InitialProject.Model;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace InitialProject.Repository.Interfaces
{
    public interface ILocationRepository : ISubject
    {
        public int NextId();
        public Language Save(Language language);
        public List<Language> GetAll();
        public Language GetById(int id);
        public void Subscribe(IObserver observer);
        public void Unsubscribe(IObserver observer);
        public void NotifyObservers();
    }
}
