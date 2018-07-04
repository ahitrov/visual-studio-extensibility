// JavaScript source code
// vsFindTarget enum from https://docs.microsoft.com/en-us/dotnet/api/envdte.vsfindtarget?view=visualstudiosdk-2017
var vsFindTarget = {
    vsFindTargetCurrentDocument: 1,
    vsFindTargetCurrentDocumentFunction: 3,
    vsFindTargetCurrentDocumentSelection: 2,
    vsFindTargetCurrentProject: 5,
    vsFindTargetFiles: 7,
    vsFindTargetOpenDocuments: 4,
    vsFindTargetSolution: 6
};

// vsFindPatternSyntax enum from https://docs.microsoft.com/en-us/dotnet/api/envdte.vsfindpatternsyntax?view=visualstudiosdk-2017
var vsFindPatternSyntax = {
    vsFindPatternSyntaxLiteral: 0,
    vsFindPatternSyntaxRegExpr: 1,
    vsFindPatternSyntaxWildcards: 2
};

// vsFindAction enum from https://docs.microsoft.com/en-us/dotnet/api/envdte.vsfindaction?view=visualstudiosdk-2017
var vsFindAction = {
    vsFindActionBookmarkAll: 5,
    vsFindActionFind: 1,
    vsFindActionFindAll: 2,
    vsFindActionReplace: 3,
    vsFindActionReplaceAll: 4
};

dte.Find.Action = vsFindAction.vsFindActionReplaceAll;
dte.Find.PatternSyntax = vsFindPatternSyntax.vsFindPatternSyntaxRegExpr;
dte.Find.MatchCase = false;
dte.Find.MatchInHiddenText = false;
dte.Find.MatchWholeWord = false;
dte.Find.Target = vsFindTarget.vsFindTargetCurrentProject;

// Remove the leading underscore in the names of all private fields
dte.Find.FindWhat = "(?<modifierAndType>\\s*private\\s*\\w*\\s*)(?<underscore>_)(?<nameWithoutUnderscore>\\w*)(?<endOfDeclaration>.*;)";
dte.Find.ReplaceWith = "${modifierAndType}${nameWithoutUnderscore}${endOfDeclaration}";

dte.Find.Execute();
