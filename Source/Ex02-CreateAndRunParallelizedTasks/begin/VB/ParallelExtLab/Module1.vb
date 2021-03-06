﻿Imports System.Threading
Imports System.Threading.Tasks

Module Module1
    Private employeeData As EmployeeList

    Sub Main(ByVal args() As String)
        employeeData = New EmployeeList()

        Console.WriteLine("Payroll process started at {0}", DateTime.Now)
        Dim sw = Stopwatch.StartNew()

        ' Methods to call
        ' Ex1Task1_ParallelizeLongRunningService()
        ' Ex1Task1_UseParallelForMethod()
        ' Ex1Task1_StandardForEach()
        ' Ex1Task1_ParallelForEach()
        Ex1Task1_WalkTree()

        Console.WriteLine("Payroll finished at {0} and took {1}", DateTime.Now, sw.Elapsed.TotalSeconds)
        Console.WriteLine()
        Console.ReadLine()
    End Sub

    Private Sub Ex1Task1_ParallelizeLongRunningService()
        Console.WriteLine("Non-parallelized for loop")

        For i = 0 To employeeData.Count - 1
            Console.WriteLine("Starting process for employee id {0}", employeeData(i).EmployeeID)
            Dim span As Decimal = PayrollServices.GetPayrollDeduction(employeeData(i))
            Console.WriteLine("Completed process for employee id {0}" & "process took {1} seconds", employeeData(i).EmployeeID, span)
            Console.WriteLine()
        Next i
    End Sub

    Private Sub Ex1Task1_UseParallelForMethod()
        Parallel.For(0, employeeData.Count,
                     Sub(i)
                         Console.WriteLine("Starting process for employee id {0}", employeeData(i).EmployeeID)
                         Dim span As Decimal = PayrollServices.GetPayrollDeduction(employeeData(i))
                         Console.WriteLine("Completed process for employee id {0}", employeeData(i).EmployeeID)
                         Console.WriteLine()
                     End Sub)
    End Sub

    Private Sub Ex1Task1_StandardForEach()
        For Each employee As Employee In employeeData
            Console.WriteLine("Starting process for employee id {0}", employee.EmployeeID)
            Dim span As Decimal = PayrollServices.GetPayrollDeduction(employee)
            Console.WriteLine("Completed process for employee id {0}", employee.EmployeeID)
            Console.WriteLine()
        Next employee
    End Sub

    Private Sub Ex1Task1_ParallelForEach()
        Parallel.ForEach(employeeData,
                         Sub(ed)
                             Console.WriteLine("Starting process for employee id {0}", ed.EmployeeID)
                             Dim span As Decimal = PayrollServices.GetPayrollDeduction(ed)
                             Console.WriteLine("Completed process for employee id {0}", ed.EmployeeID)
                             Console.WriteLine()
                         End Sub)
    End Sub

    Private Sub Ex1Task1_WalkTree()
        Dim employeeHierarchy As New EmployeeHierarchy()
        WalkTree(employeeHierarchy)
    End Sub

    Private Sub WalkTree(ByVal node As Tree(Of Employee))
        If node Is Nothing Then
            Return
        End If

        If node.Data IsNot Nothing Then
            Dim emp As Employee = node.Data
            Console.WriteLine("Starting process for employee id {0}", emp.EmployeeID)
            Dim span As Decimal = PayrollServices.GetPayrollDeduction(emp)
            Console.WriteLine("Completed process for employee id {0}", emp.EmployeeID)
            Console.WriteLine()
        End If

        Parallel.Invoke(Sub() WalkTree(node.Left), Sub() WalkTree(node.Right))
    End Sub
End Module
