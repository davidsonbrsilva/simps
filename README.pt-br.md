# Simps

![Licença](https://img.shields.io/github/license/davidsonbrsilva/simps.svg) ![Status](https://img.shields.io/badge/status-stopped-red)

[![Captura de tela do funcionamento do Simps](cover.png)](https://www.youtube.com/watch?v=HeeFFVe0gBA)

[[See in English](README.md)]

**Simps** (pronuncia-se `/sɪmps/`) é um simulador de processos semióticos escrito em C# que utiliza [Unity](https://unity3d.com) como motor de desenvolvimento. Este projeto foi parte da linha de pesquisa _Modelagem e Simulação Multiagente de Processos Semióticos: semântica artificial_ desenvolvida pelo Grupo de Estudos em Linguagem, Cognição e Computação (LC2) da [Universidade Federal dos Vales do Jequitinhonha e Mucuri](http://www.ufvjm.edu.br).

Simps é um projeto de inteligência artificial inspirado no caso etológico dos macacos _vervets_ proposto por [Loula et. al. (2004)](https://www.dca.fee.unicamp.br/~gudwin/ftp/publications/TeseLoula.pdf) em que presas e predadores coexistem em um ambiente virtual e interagem entre si através de processos básicos como memória associativa, percepção e foco de atenção. Às presas é fornecido um conjunto de palavras (léxicos) para informarem sobre eventos ocorridos como, por exemplo, ver um predador. A informação é disparada ao ambiente e pode ser percebida por outras presas por meio de seus sensores de audição, que reagirão conforme a interpretação obtida pelo seu processamento. Ao final, um léxico comum se constrói de tais interações e, se bem utilizado, oferece vantagens de sobrevivência às presas. Dizemos, neste caso, que as presas aprenderam a se comunicar por meio de uma _linguagem emergente_.

Para mais detalhes do projeto, consulte o [Trabalho de Conclusão de Curso](https://drive.google.com/file/d/1RpTITqPAhEirOiVWzSS7sNMw1LzWqGAu/view?usp=sharing) dos autores.

## Começando

As instruções a seguir mostram como você pode obter uma cópia deste projeto e rodá-lo em sua máquina local para propósitos de desenvolvimento e testes.

### Pré-requisitos

- [Unity 2020.1.15f1](https://unity.com/releases/editor/archive)
- IDE de apoio, como [Visual Studio](https://www.visualstudio.com/pt-br/downloads/) ou [MonoDevelop](http://www.monodevelop.com/download/)
  
> Para o Visual Studio, é necessário habilitar o suporte ao Unity nos itens de instalação.

### Instalação

Faça o clone do repositório:

```
$ git clone https://github.com/davidsonbrsilva/simps.git
```

### Instruções de uso

Para executar simulações no Simps:

1. Navegue até `project/Assets/Scenes` e abra o arquivo `simulator.unity`. Você pode mudar as configurações padrão das simulações alterando as propriedades do objeto `Core` da aba `Hierarchy` através do `Inspector`.
2. Clique em `Play`.

## Autores

- Davidson Bruno da Silva <<davidsonbruno@outlook.com>>
- Leonardo Lana de Carvalho <<lanadecarvalholeonardo@gmail.com>>

Agradecimento especial a [Tiago Ferreira Campos](https://github.com/caotic123) e [Lucas Vieira Souza](https://github.com/luksamuk) por acompanharem todo o processo de desenvolvimento, contribuirem com dicas e disporem de tempo para nos ajudar pessoalmente.

## Licença

[MIT](LICENSE) Copyright (c) 2020, Davidson Bruno.
