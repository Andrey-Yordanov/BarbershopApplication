using System.CodeDom;

namespace Barbershop.Common
{
    public static class EntityValidationConstants
    {
        public static class Category
        {
            public const int CategoryNameMinLength = 3;
            public const int CategoryNameMaxLength = 50;
        }
        public static class Service
        {
            public const int ServiceNameMinLength = 3;
            public const int ServiceNameMaxLength = 50;

            public const int ServiceDescriptionMinLength = 20;
            public const int ServiceDescriptionMaxLength = 200;
        }
        public static class ApplicationUser
        {
            public const int UserFirstNameMinLength = 3;
            public const int UserFirstNameMaxLength = 50;

            public const int UserLastNameMinLength = 3;
            public const int UserLastNameMaxLength = 50;
        }
        public static class ContactMessage
        {
            public const int ContactNameMinLength = 3;
            public const int ContactNameMaxLength = 100;

            public const int ContactEmailMinLength = 5;
            public const int ContactEmailMaxLength = 100;

            public const int ContactMessageMinLength = 10;
            public const int ContactMessageMaxLength = 400;
        }
    }
}
