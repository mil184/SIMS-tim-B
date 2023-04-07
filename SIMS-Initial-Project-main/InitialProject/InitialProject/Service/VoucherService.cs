using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;

namespace InitialProject.Service
{
    public class VoucherService : ISubject
    {
        private readonly VoucherRepository _voucherRepository;

        public VoucherService()
        {
            _voucherRepository = new VoucherRepository();
        }

        public void Subscribe(IObserver observer)
        {
            _voucherRepository.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _voucherRepository.Unsubscribe(observer);
        }

        public void NotifyObservers()
        {
            _voucherRepository.NotifyObservers();
        }
    }
}
