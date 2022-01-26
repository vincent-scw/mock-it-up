using MockItUp.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MockItUp.Core
{
    public class HitRecordCollection : IReadOnlyCollection<HitRecord>
    {
        private List<HitRecord> _records = new List<HitRecord>();

        public int Count => _records.Count;

        public IEnumerator<HitRecord> GetEnumerator()
        {
            return _records.GetEnumerator();
        }

        public void Record(RequestModel request, ResponseModel response, StubItem stub)
        {
            _records.Add(new HitRecord 
            { 
                Request = request, 
                Response = response,
                Stub = stub,
                RecordTime = DateTimeOffset.Now 
            });
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _records.GetEnumerator();
        }
    }
}
