using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InitialProject.Service
{
    public class VoucherService : ISubject
    {
        private readonly VoucherRepository _voucherRepository;

        public VoucherService()
        {
            _voucherRepository = new VoucherRepository();
        }

        public ObservableCollection<Voucher> GetUserVouchers(User user)
        {
            ObservableCollection<Voucher> vouchers = new ObservableCollection<Voucher>();
            List<Voucher> allVouchers = _voucherRepository.GetAll();

            foreach(Voucher voucher in allVouchers)
            {
                if (voucher.UserId == user.Id)
                {
                    vouchers.Add(voucher);
                }
            }

            return vouchers;
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
