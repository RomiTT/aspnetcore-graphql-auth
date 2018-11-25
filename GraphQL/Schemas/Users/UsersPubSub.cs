using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using aspnetcore_graphql_auth.Models;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users {
    public class UsersPubSub {
        private readonly Subject<User> _subjectOnSignup = new Subject<User>();
        private readonly Subject<User> _subjectOnLogin = new Subject<User>();
        private readonly Subject<User> _subjectOnLogout = new Subject<User>();
        private readonly Subject<User> _subjectOnDeleteUser = new Subject<User>();
        private readonly Subject<User> _subjectOnAllEvents = new Subject<User>();

        public IObservable<User> ObservableOnSignup => _subjectOnSignup.AsObservable();
        public IObservable<User> ObservableOnLogin => _subjectOnLogin.AsObservable();
        public IObservable<User> ObservableOnLogout => _subjectOnLogout.AsObservable();
        public IObservable<User> ObservableOnDeleteUser => _subjectOnDeleteUser.AsObservable();
        public IObservable<User> ObservableOnAllEvents => _subjectOnAllEvents.AsObservable();

        public UsersPubSub() {
            _subjectOnSignup.Subscribe(_subjectOnAllEvents);
            _subjectOnLogin.Subscribe(_subjectOnAllEvents);
            _subjectOnLogout.Subscribe(_subjectOnAllEvents);
            _subjectOnDeleteUser.Subscribe(_subjectOnAllEvents);
        }

        public void SignupUser(User user) {
            _subjectOnSignup.OnNext(user);
        }

        public void LoginUser(User user) {
            _subjectOnLogin.OnNext(user);
        }

        public void LogoutUser(User user) {
            _subjectOnLogout.OnNext(user);
        }

        public void DeleteUser(User user) {
            _subjectOnDeleteUser.OnNext(user);
        }
    }
}