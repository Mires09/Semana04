const API_URL = "https://scaling-cod-5ggrgv45j7g5h4479-8000.app.github.dev";

function limparMensagem(id) {
  const elemento = document.getElementById(id);
  if (elemento) {
    elemento.textContent = "";
    elemento.className = "mensagem";
  }
}

function exibirMensagem(id, texto, tipo) {
  const elemento = document.getElementById(id);
  if (elemento) {
    elemento.textContent = texto;
    elemento.className = `mensagem ${tipo}`;
  }
}

function limparFormularioCadastro() {
  document.getElementById("nome").value = "";
  document.getElementById("idade").value = "";
  document.getElementById("cargo").value = "";
  document.getElementById("departamento").value = "";
  document.getElementById("salario").value = "";
}

function normalizarFuncionario(funcionario) {
  return {
    id: funcionario.id ?? funcionario.Id,
    nome: funcionario.nome ?? funcionario.Nome,
    idade: funcionario.idade ?? funcionario.Idade,
    cargo: funcionario.cargo ?? funcionario.Cargo,
    departamento: funcionario.departamento ?? funcionario.Departamento,
    salario: funcionario.salario ?? funcionario.Salario,
  };
}

function exibirFuncionarios(funcionarios) {
  const resultado = document.getElementById("resultado");
  if (!resultado) return;

  if (!funcionarios || funcionarios.length === 0) {
    resultado.innerHTML = "<p>Nenhum funcionário encontrado.</p>";
    return;
  }

  const funcionariosValidos = funcionarios.filter((item) => item !== null);
  if (funcionariosValidos.length === 0) {
    resultado.innerHTML = "<p>Nenhum funcionário encontrado.</p>";
    return;
  }

  let tabela = `
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Nome</th>
          <th>Idade</th>
          <th>Cargo</th>
          <th>Departamento</th>
          <th>Salário</th>
        </tr>
      </thead>
      <tbody>
  `;

  funcionariosValidos.forEach((item) => {
    const funcionario = normalizarFuncionario(item);
    tabela += `
      <tr>
        <td>${funcionario.id}</td>
        <td>${funcionario.nome}</td>
        <td>${funcionario.idade}</td>
        <td>${funcionario.cargo}</td>
        <td>${funcionario.departamento}</td>
        <td>R$ ${Number(funcionario.salario).toFixed(2)}</td>
      </tr>
    `;
  });

  tabela += `
      </tbody>
    </table>
  `;
  resultado.innerHTML = tabela;
}

function exibirJson(dados) {
  const resultado = document.getElementById("resultado");
  if (resultado) {
    resultado.innerHTML = `<pre>${JSON.stringify(dados, null, 2)}</pre>`;
  }
}

async function tratarResposta(response) {
  const dados = await response.json().catch(() => null);
  if (!response.ok) {
    const mensagem =
      dados?.message || dados?.mensagem || "Erro ao executar operação.";
    throw new Error(mensagem);
  }
  return dados;
}

async function testarApi() {
  limparMensagem("statusApi");
  try {
    const response = await fetch(`${API_URL}/`);
    const texto = await response.text();
    exibirMensagem("statusApi", texto, "sucesso");
  } catch (error) {
    exibirMensagem("statusApi", "Erro ao conectar com a API.", "erro");
  }
}

// Cadastro
async function cadastrarFuncionario() {
  limparMensagem("mensagemCadastro");

  const funcionario = {
    nome: document.getElementById("nome").value,
    idade: Number(document.getElementById("idade").value),
    cargo: document.getElementById("cargo").value,
    departamento: document.getElementById("departamento").value,
    salario: Number(document.getElementById("salario").value),
  };

  if (
    !funcionario.nome ||
    !funcionario.idade ||
    !funcionario.cargo ||
    !funcionario.departamento ||
    !funcionario.salario
  ) {
    exibirMensagem("mensagemCadastro", "Preencha todos os campos.", "erro");
    return;
  }

  try {
    const response = await fetch(`${API_URL}/cadastrarfuncionarios`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(funcionario),
    });

    const dados = await tratarResposta(response);
    exibirMensagem(
      "mensagemCadastro",
      "Funcionário cadastrado com sucesso.",
      "sucesso"
    );
    exibirJson(dados);
    limparFormularioCadastro();
  } catch (error) {
    exibirMensagem("mensagemCadastro", error.message, "erro");
  }
}

// Listagem
async function listarFuncionarios() {
  try {
    const response = await fetch(`${API_URL}/listarfuncionarios`);
    const dados = await tratarResposta(response);
    const funcionarios =
      dados.funcionariosCadastrados || dados.FuncionariosCadastrados;
    exibirFuncionarios(funcionarios);
  } catch (error) {
    const resultado = document.getElementById("resultado");
    if (resultado) {
      resultado.innerHTML = `<p class="erro">${error.message}</p>`;
    }
  }
}

// Atualização completa
async function atualizarFuncionario() {
  limparMensagem("mensagemAtualizacao");

  const id = document.getElementById("putId").value;
  const funcionario = {
    nome: document.getElementById("putNome").value,
    idade: Number(document.getElementById("putIdade").value),
    cargo: document.getElementById("putCargo").value,
    departamento: document.getElementById("putDepartamento").value,
    salario: Number(document.getElementById("putSalario").value),
  };

  if (
    !id ||
    !funcionario.nome ||
    !funcionario.idade ||
    !funcionario.cargo ||
    !funcionario.departamento ||
    !funcionario.salario
  ) {
    exibirMensagem("mensagemAtualizacao", "Preencha todos os campos.", "erro");
    return;
  }

  try {
    const response = await fetch(`${API_URL}/atualizarfuncionario/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(funcionario),
    });

    const dados = await tratarResposta(response);
    exibirMensagem(
      "mensagemAtualizacao",
      "Funcionário atualizado com sucesso.",
      "sucesso"
    );
    exibirJson(dados);
  } catch (error) {
    exibirMensagem("mensagemAtualizacao", error.message, "erro");
  }
}

// Atualização parcial (salário)
async function atualizarSalario() {
  limparMensagem("mensagemSalario");

  const id = document.getElementById("patchId").value;
  const salario = Number(document.getElementById("patchSalario").value);

  if (!id || !salario) {
    exibirMensagem("mensagemSalario", "Informe o ID e o novo salário.", "erro");
    return;
  }

  try {
    const response = await fetch(`${API_URL}/atualizarfuncionario/${id}`, {
      method: "PATCH",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ salario }),
    });

    const dados = await tratarResposta(response);
    exibirMensagem(
      "mensagemSalario",
      "Salário atualizado com sucesso.",
      "sucesso"
    );
    exibirJson(dados);
  } catch (error) {
    exibirMensagem("mensagemSalario", error.message, "erro");
  }
}

// Remoção
async function removerFuncionario() {
  limparMensagem("mensagemRemocao");

  const id = document.getElementById("deleteId").value;
  if (!id) {
    exibirMensagem("mensagemRemocao", "Informe o ID do funcionário.", "erro");
    return;
  }

  try {
    const response = await fetch(`${API_URL}/deletarfuncionario/${id}`, {
      method: "DELETE",
    });

    const dados = await tratarResposta(response);
    exibirMensagem(
      "mensagemRemocao",
      "Funcionário removido com sucesso.",
      "sucesso"
    );
    exibirJson(dados);
  } catch (error) {
    exibirMensagem("mensagemRemocao", error.message, "erro");
  }
}

function abrirFecharMenu() {
  const menuLinks = document.getElementById("menuLinks");
  menuLinks.classList.toggle("ativo");
}
