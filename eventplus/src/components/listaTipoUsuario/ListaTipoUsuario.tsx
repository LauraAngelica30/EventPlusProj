import type { TipoUsuario, Usuario } from "../../types/api";
import "./ListaTipoUsuario.css"

import lapis from "../../assets/icons/lapis.svg";
import lixeira from  "../../assets/icons/lixeira.svg";

interface ListaTipoUsuarioProps {
    tipoUsuario: TipoUsuario[];
    idEmEdicao: string | null;
    carregando?: boolean;
    onEditar: (tipoUsuario: TipoUsuario) => void;
    onExcluir: (tipoUsuario: TipoUsuario) => void;
}

function ListaTipoUsuario({ tipoUsuario, idEmEdicao, carregando, onEditar, onExcluir }: ListaTipoUsuarioProps) {
    return (
        <section className="lista-tipos lista-tipoUsuario">
            <div className="lista-tipos-conteudo">
                <h2>Tipos de Usuarios Cadastrados</h2>
                <div className="lista-tipos-tabela-wrapper">
                    <table className="lista-tipoUsuario-tabela">
                        <thead>
                            <tr>
                                <th>Título</th>
                                <th>Editar</th>
                                <th>Excluir</th>
                            </tr>
                        </thead>
                        <tbody>
                            {carregando
                                ? <tr><td colSpan={5} className="lista-tipos-tabela-vazia">Carregando...</td></tr>
                                : null
                            }
                            {!carregando && tipoUsuario.length === 0
                                ? <tr><td colSpan={5} className="lista-tipos-tabela-vazia">Nenhum tipo cadastrado</td></tr>
                                : null
                            }
                            {!carregando && tipoUsuario.map((tipo) => (
                                <tr className={idEmEdicao === tipo.idTipoUsuario ? "lista-tipos-linha-ativa" : ""} key={tipo.idTipoUsuario}>
                                    <td>{tipo.titulo}</td>
                                    <td><button className="lista-tipos-acao" type="button" aria-label={`Editar ${tipo.titulo}`} onClick={() => onEditar(tipo)}><img src={lapis} alt="icone de editar"/></button></td>
                                    <td><button className="lista-tipos-acao" type="button" aria-label={`Excluir ${tipo.titulo}`} onClick={() => onExcluir(tipo)}><img src={lixeira} alt="icone de excluir"/></button></td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>

            </div>
        </section>
    )

}

export default ListaTipoUsuario;