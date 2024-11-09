using System.Text.Json;
using user_ms.Src.Models;

namespace user_ms.Src.Data
{
    public class Seed
    {
        /// <summary>
        /// Seed the database with examples models in the json files if the database is empty.
        /// </summary>
        /// <param name="context">Database Context </param>
        public static void SeedData(DataContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            CallEachSeeder(context, options);
        }

        /// <summary>
        /// Centralize the call to each seeder method
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        public static void CallEachSeeder(DataContext context, JsonSerializerOptions options)
        {
            SeedFirstOrderTables(context, options);
            SeedSecondtOrderTables(context, options);
        }

        /// <summary>
        /// Seed the database with the tables that don't depend on other tables.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedFirstOrderTables(DataContext context, JsonSerializerOptions options)
        {
            SeedRoles(context, options);
            SeedSubjects(context, options);
            SeedCareers(context, options);
            SeedUsers(context, options);
        }

        /// <summary>
        /// Seed the database with the tables whose data depends on exatly one table.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedSecondtOrderTables(DataContext context, JsonSerializerOptions options)
        {
            SeedSubjectsRelationships(context, options);
        }

        /// <summary>
        /// Seed the database with the roles in the json file and save changes if the database is empty.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedRoles(DataContext context, JsonSerializerOptions options)
        {
            var result = context.Roles?.Any();
            if (result is true or null) return;

            var path = "Src/Data/DataSeeders/RolesData.json";
            var rolesData = File.ReadAllText(path);
            var rolesList = JsonSerializer.Deserialize<List<Role>>(rolesData, options) ??
                throw new Exception("RolesData.json is empty");


            // Normalize DateTime properties to UTC
            rolesList.ForEach(r =>
            {
                r.CreatedAt = DateTime.SpecifyKind(r.CreatedAt, DateTimeKind.Utc);
                r.UpdatedAt = DateTime.SpecifyKind(r.UpdatedAt, DateTimeKind.Utc);
                if (r.DeletedAt.HasValue)
                {
                    r.DeletedAt = DateTime.SpecifyKind(r.DeletedAt.Value, DateTimeKind.Utc);
                }
            });

            context.Roles?.AddRange(rolesList);
            context.SaveChanges();
        }

        /// <summary>
        /// Seed the database with the subjects in the json file and save changes if the database is empty.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedSubjects(DataContext context, JsonSerializerOptions options)
        {
            var result = context.Subjects?.Any();
            if (result is true or null) return;

            var path = "Src/Data/DataSeeders/SubjectsData.json";
            var subjectsData = File.ReadAllText(path);
            var subjectsList = JsonSerializer.Deserialize<List<Subject>>(subjectsData, options) ??
                throw new Exception("SubjectsData.json is empty");
            // Normalize the name, code and department of the subjects
            subjectsList.ForEach(s =>
            {
                s.Code = s.Code.ToLower();
                s.Name = s.Name.ToLower();
                s.Department = s.Department.ToLower();

                s.CreatedAt = DateTime.SpecifyKind(s.CreatedAt, DateTimeKind.Utc);
                s.UpdatedAt = DateTime.SpecifyKind(s.UpdatedAt, DateTimeKind.Utc);
                if (s.DeletedAt.HasValue)
                {
                    s.DeletedAt = DateTime.SpecifyKind(s.DeletedAt.Value, DateTimeKind.Utc);
                }
            });

            context.Subjects?.AddRange(subjectsList);
            context.SaveChanges();
        }

        /// <summary>
        /// Seed the database with the careers in the json file and save changes if the database is empty.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedCareers(DataContext context, JsonSerializerOptions options)
        {
            var result = context.Careers?.Any();
            if (result is true or null) return;
            var path = "Src/Data/DataSeeders/CareersData.json";
            var careersData = File.ReadAllText(path);
            var careersList = JsonSerializer.Deserialize<List<Career>>(careersData, options) ??
                throw new Exception("CareersData.json is empty");
            // Normalize the name and code of the careers
            careersList.ForEach(s =>
            {
                s.Name = s.Name.ToLower();

                s.CreatedAt = DateTime.SpecifyKind(s.CreatedAt, DateTimeKind.Utc);
                s.UpdatedAt = DateTime.SpecifyKind(s.UpdatedAt, DateTimeKind.Utc);
                if (s.DeletedAt.HasValue)
                {
                    s.DeletedAt = DateTime.SpecifyKind(s.DeletedAt.Value, DateTimeKind.Utc);
                }
            });

            context.Careers?.AddRange(careersList);
            context.SaveChanges();
        }

        /// <summary>
        /// Seed the database with the subjects relationships in the json file and save changes if the database is empty.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedSubjectsRelationships(DataContext context, JsonSerializerOptions options)
        {
            var result = context.SubjectRelationships?.Any();
            if (result is true or null) return;
            var path = "Src/Data/DataSeeders/SubjectsRelationsData.json";
            var subjectsRelationshipsData = File.ReadAllText(path);
            var subjectsRelationshipsList = JsonSerializer
                .Deserialize<List<SubjectRelationship>>(subjectsRelationshipsData, options) ??
                throw new Exception("SubjectsRelationsData.json is empty");
            // Normalize the codes of the codes
            subjectsRelationshipsList.ForEach(s =>
            {
                s.SubjectCode = s.SubjectCode.ToLower();
                s.PreSubjectCode = s.PreSubjectCode.ToLower();

                s.CreatedAt = DateTime.SpecifyKind(s.CreatedAt, DateTimeKind.Utc);
                s.UpdatedAt = DateTime.SpecifyKind(s.UpdatedAt, DateTimeKind.Utc);
                if (s.DeletedAt.HasValue)
                {
                    s.DeletedAt = DateTime.SpecifyKind(s.DeletedAt.Value, DateTimeKind.Utc);
                }
            });

            context.SubjectRelationships?.AddRange(subjectsRelationshipsList);
            context.SaveChanges();
        }

        /// </summary>
        /// Seed the database with the users in the json file and save changes if the database is empty.
        /// </summary> 
        /// <param name="context">Database Context</param>
        /// <param name="options">Options to deserialize json</param>
        private static void SeedUsers(DataContext context, JsonSerializerOptions options)
        {
            var result = context.Users?.Any();
            if (result is true or null) return;
            var path = "Src/Data/DataSeeders/UsersData.json";
            var usersData = File.ReadAllText(path);
            var usersList = JsonSerializer.Deserialize<List<User>>(usersData, options) ??
                throw new Exception("UsersData.json is empty");
            // Normalize the name and code of the careers
            usersList.ForEach(s =>
            {
                s.Name = s.Name.ToLower();
                s.FirstLastName = s.FirstLastName.ToLower();
                s.SecondLastName = s.SecondLastName.ToLower();
                s.CreatedAt = DateTime.SpecifyKind(s.CreatedAt, DateTimeKind.Utc);
                s.UpdatedAt = DateTime.SpecifyKind(s.UpdatedAt, DateTimeKind.Utc);
                if (s.DeletedAt.HasValue)
                {
                    s.DeletedAt = DateTime.SpecifyKind(s.DeletedAt.Value, DateTimeKind.Utc);
                }
            });

            context.Users?.AddRange(usersList);
            context.SaveChanges();
        }
    }
}