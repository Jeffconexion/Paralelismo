﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using ByteBank.Core.Repository;
using ByteBank.Core.Service;

namespace ByteBank.View
{
  public partial class MainWindow : Window
  {
    private readonly ContaClienteRepository r_Repositorio;
    private readonly ContaClienteService r_Servico;

    public MainWindow()
    {
      InitializeComponent();

      r_Repositorio = new ContaClienteRepository();
      r_Servico = new ContaClienteService();
    }

    private void BtnProcessar_Click(object sender, RoutedEventArgs e)
    {
      var contas = r_Repositorio.GetContaClientes();

      var contasQuantidadePorThread = contas.Count() / 4;

      var contas_parte1 = contas.Take(contasQuantidadePorThread);
      var contas_parte2 = contas.Skip(contasQuantidadePorThread).Take(contasQuantidadePorThread);
      var contas_parte3 = contas.Take(contasQuantidadePorThread * 2).Take(contasQuantidadePorThread);
      var contas_parte4 = contas.Take(contasQuantidadePorThread * 3);


      var resultado = new List<string>();

      AtualizarView(new List<string>(), TimeSpan.Zero);

      var inicio = DateTime.Now;

      Thread thread_parte1 = new Thread(() =>
      {
        foreach (var conta in contas_parte1)
        {
          var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
          resultado.Add(resultadoConta);
        }
      });

      Thread thread_parte2 = new Thread(() =>
      {
        foreach (var conta in contas_parte2)
        {
          var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
          resultado.Add(resultadoConta);
        }
      });

      Thread thread_parte3 = new Thread(() =>
      {
        foreach (var conta in contas_parte3)
        {
          var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
          resultado.Add(resultadoConta);
        }
      });

      Thread thread_parte4 = new Thread(() =>
      {
        foreach (var conta in contas_parte4)
        {
          var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
          resultado.Add(resultadoConta);
        }
      });

      //dot net cria uma thread principal
      //5 threads ao total
      thread_parte1.Start();
      thread_parte2.Start();
      thread_parte3.Start();
      thread_parte4.Start();

      while (thread_parte1.IsAlive || thread_parte2.IsAlive || thread_parte3.IsAlive || thread_parte4.IsAlive)
      {
        //coloco a thread principal do dotnet para dormir.
        Thread.Sleep(250);
      }


      var fim = DateTime.Now;

      AtualizarView(resultado, fim - inicio);
    }

    private void AtualizarView(List<String> result, TimeSpan elapsedTime)
    {
      var tempoDecorrido = $"{ elapsedTime.Seconds }.{ elapsedTime.Milliseconds} segundos!";
      var mensagem = $"Processamento de {result.Count} clientes em {tempoDecorrido}";

      LstResultados.ItemsSource = result;
      TxtTempo.Text = mensagem;
    }
  }
}
