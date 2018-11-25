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
        private readonly Subject<String> _subjectOnAllEvents = new Subject<String>();

        public IObservable<User> ObservableOnSignup => _subjectOnSignup.AsObservable();
        public IObservable<User> ObservableOnLogin => _subjectOnLogin.AsObservable();
        public IObservable<User> ObservableOnLogout => _subjectOnLogout.AsObservable();
        public IObservable<User> ObservableOnDeleteUser => _subjectOnDeleteUser.AsObservable();
        public IObservable<String> ObservableOnAllEvents => _subjectOnAllEvents.AsObservable();

        public UsersPubSub() {
            _subjectOnSignup.Select(u => $"Signup: {u.Email}").Subscribe(_subjectOnAllEvents);
            _subjectOnLogin.Select(u => $"Login: {u.Email}").Subscribe(_subjectOnAllEvents);
            _subjectOnLogout.Select(u => $"Logout: {u.Email}").Subscribe(_subjectOnAllEvents);
            _subjectOnDeleteUser.Select(u => $"DeleteUser: {u.Email}").Subscribe(_subjectOnAllEvents);
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