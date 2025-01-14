﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
      var taskSchedulerUI = TaskScheduler.FromCurrentSynchronizationContext();
      BtnProcessar.IsEnabled = false;


      var contas = r_Repositorio.GetContaClientes();

      var resultado = new List<string>();

      AtualizarView(new List<string>(), TimeSpan.Zero);

      var inicio = DateTime.Now;

      var contasTarefas = contas.Select(conta =>
      {
        return Task.Factory.StartNew(() =>
        {
          var resultadoConta = r_Servico.ConsolidarMovimentacao(conta);
          resultado.Add(resultadoConta);

        });
      }).ToArray();

      Task.WhenAll(contasTarefas)// uma task só para esperar as outras.
           .ContinueWith(task =>
           {
             var fim = DateTime.Now;
             AtualizarView(resultado, fim - inicio);
           }, taskSchedulerUI)
           .ContinueWith(task =>
           {
             BtnProcessar.IsEnabled = true;
           }, taskSchedulerUI);
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
