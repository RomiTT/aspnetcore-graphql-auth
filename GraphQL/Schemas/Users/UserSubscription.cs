using System;
using System.Reactive.Linq;
using aspnetcore_graphql_auth.GraphQL.Schemas.Users.Type.Output;
using aspnetcore_graphql_auth.Models;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Server.Transports.Subscriptions.Abstractions;
using GraphQL.Subscription;
using GraphQL.Types;

namespace aspnetcore_graphql_auth.GraphQL.Schemas.Users {
    public class UsersSubscription : ObjectGraphType {
        UsersPubSub _pubsub;
        public UsersSubscription(UsersPubSub pubsub) {
            _pubsub = pubsub;

            AddField(new EventStreamFieldType {
                Name = "subscribeOnSignup",
                    Type = typeof(UserType),
                    Resolver = new FuncFieldResolver<User>(ResolveUser),
                    Subscriber = new EventStreamResolver<User>(SubscribeOnSignup)
            });

            AddField(new EventStreamFieldType {
                Name = "subscribeOnLogin",
                    Type = typeof(UserType),
                    Resolver = new FuncFieldResolver<User>(ResolveUser),
                    Subscriber = new EventStreamResolver<User>(SubscribeOnLogin)
            });

            AddField(new EventStreamFieldType {
                Name = "subscribeOnLoginByUser",
                    Arguments = new QueryArguments(
                        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" }
                    ),

                    Type = typeof(UserType),
                    Resolver = new FuncFieldResolver<User>(ResolveUser),
                    Subscriber = new EventStreamResolver<User>(SubscribeOnLogin)
            });

            AddField(new EventStreamFieldType {
                Name = "subscribeOnLogout",
                    Type = typeof(UserType),
                    Resolver = new FuncFieldResolver<User>(ResolveUser),
                    Subscriber = new EventStreamResolver<User>(SubscribeOnLogout)
            });

            AddField(new EventStreamFieldType {
                Name = "subscribeOnDeleteUser",
                    Type = typeof(UserType),
                    Resolver = new FuncFieldResolver<User>(ResolveUser),
                    Subscriber = new EventStreamResolver<User>(SubscribeOnDeleteUser)
            });

            AddField(new EventStreamFieldType {
                Name = "subscribeOnAllEvents",
                    Type = typeof(StringGraphType),
                    Resolver = new FuncFieldResolver<String>(ResolveString),
                    Subscriber = new EventStreamResolver<String>(SubscribeOnAllEvents)
            });

        }

        private User ResolveUser(ResolveFieldContext context) {
            var user = context.Source as User;
            return user;
        }

        private String ResolveString(ResolveFieldContext context) {
            var result = context.Source as String;
            return result;
        }

        private IObservable<User> SubscribeOnSignup(ResolveEventStreamContext context) {
            var messageContext = context.UserContext.As<MessageHandlingContext>();
            var email = messageContext.Get<string>("email");

            return _pubsub.ObservableOnSignup;
        }

        private IObservable<User> SubscribeOnLogin(ResolveEventStreamContext context) {
            var messageContext = context.UserContext.As<MessageHandlingContext>();
            var email = messageContext.Get<string>("email");

            return _pubsub.ObservableOnLogin;
        }

        private IObservable<User> SubscribeOnLoginByUser(ResolveEventStreamContext context) {
            var messageContext = context.UserContext.As<MessageHandlingContext>();
            var email = messageContext.Get<string>("email");

            var argEmail = context.GetArgument<string>("email");

            return _pubsub.ObservableOnLogin.Where(user => user.Email == argEmail);
        }

        private IObservable<User> SubscribeOnLogout(ResolveEventStreamContext context) {
            var messageContext = context.UserContext.As<MessageHandlingContext>();
            var email = messageContext.Get<string>("email");

            return _pubsub.ObservableOnLogout;
        }

        private IObservable<User> SubscribeOnDeleteUser(ResolveEventStreamContext context) {
            var messageContext = context.UserContext.As<MessageHandlingContext>();
            var email = messageContext.Get<string>("email");

            return _pubsub.ObservableOnDeleteUser;
        }

        private IObservable<String> SubscribeOnAllEvents(ResolveEventStreamContext context) {
            var messageContext = context.UserContext.As<MessageHandlingContext>();
            var email = messageContext.Get<string>("email");

            return _pubsub.ObservableOnAllEvents;
        }

    }
}