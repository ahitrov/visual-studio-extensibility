namespace VSIXExample
{
    public static class RegexConstants
    {
        public static class GroupNames
        {
            public const string ModifierAndType = nameof(ModifierAndType);
            public const string Underscore = nameof(Underscore);
            public const string NameWithoutUnderscore = nameof(NameWithoutUnderscore);
            public const string EndOfDeclaration = nameof(EndOfDeclaration);
        }

        public static string MatchLeadingUnderscoreInPrivateFieldDeclarations =
            $"(?<{GroupNames.ModifierAndType}>\\s*private\\s*\\w*\\s*)(?<{GroupNames.Underscore}>_)(?<{GroupNames.NameWithoutUnderscore}>\\w*)(?<{GroupNames.EndOfDeclaration}>.*;)";
        public static string RemoveLeadingUnderscoreReplacePattern = $"${{{GroupNames.ModifierAndType}}}${{{GroupNames.NameWithoutUnderscore}}}${{{GroupNames.EndOfDeclaration}}}";
    }
}
