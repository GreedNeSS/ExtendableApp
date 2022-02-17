Imports CommonSnappableTypes

<CompanyInfo(CompanyName:="Chucky' s Software", CompanyUrl:="www.ChuckySoft.com")>
Public Class VbSnapIn
    Implements IAppFunctionality

    Private Sub IAppFunctionality_DoIt() Implements IAppFunctionality.DoIt
        Console.WriteLine("You have just used the VB snap in 1 ")
    End Sub
End Class

