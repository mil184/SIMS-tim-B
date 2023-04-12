using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Resources.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InitialProject.Service
{
    public class VoucherService : ISubject
    {
        private readonly VoucherRepository _voucherRepository;

        public VoucherService()
        {
            _voucherRepository = new VoucherRepository();
        }

        public List<Voucher> GetUserVouchers(User user)
        {
            List<Voucher> allVouchers = _voucherRepository.GetAll();
            return allVouchers.FindAll(voucher => voucher.UserId == user.Id);
        }

        public List<Voucher> GetActiveVouchers(List<Voucher> vouchers)
        {
            return vouchers.FindAll(voucher => voucher.IsActive);
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
        public Voucher GetById(int id)
        {
           return _voucherRepository.GetById(id);
        }
        public Voucher Save(Voucher voucher) 
        {
            return _voucherRepository.Save(voucher);
        }
        public Voucher Update(Voucher voucher)
        {
            return _voucherRepository.Update(voucher);
        }
    }
}
