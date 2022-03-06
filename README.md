# Parallelism in C#

### Synopsis ###
<p> 
  ByteBank performs daily customer transactions and financial calculations. However, she acts very slowly. 
  In this project I tried to optimize this processing time through the use of parallelism. 
</p>

### Topics ###
* 1 - Use Threads.
* 2 - Use Tasks.
* 3 - Use AsyncAwait.
* 4 - Task progress notification patterns.
* 5 - Creating cancelable tasks.


### Project ###
<p> 
    We have an application running, ByteBank. It is a bank like the ones we are used to, responsible for taking care of checking accounts, investments, banking transactions in      general. Our application consolidates this data, collecting all customer transactions at the end of the day and making various financial calculations, adjustments, and returning them later to ByteBank users and employees.
</p> 

<!--
<p align="center">
  <img src="https://github.com/Jeffconexion/projeto_gerenciadorDeTarefa_angular/blob/main/projeto.gif" />
</p>

-->
