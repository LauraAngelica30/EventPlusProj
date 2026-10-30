import { Navigate, Route, Routes } from "react-router-dom";

import Home from "../pages/Home/Home"
import Eventos from "../pages/Eventos/Eventos"
import DetalhesEvento from "../pages/detalhesEvento/DetalhesEvento";
import Login from "../pages/Login/Login";
import Usuarios from "../pages/Usuarios/Usuarios";
import TipoEventoPage from "../pages/TipoEvento/TipoEvento";
import TipoUsuarioPage from "../pages/TipoUsuario/TipoUsuario";
import CadastroEvento from "../pages/CadastroEvento/CadastroEvento";
import ProtectedRoute from "./ProtectedRoute";

function AppRoutes() {
    return(
        <Routes>
            <Route path="/home" element={<Home/>} />

            <Route path="/eventos" element={<Eventos/>} />

            <Route path="/tipoEvento" element={<TipoEventoPage/>} />

            <Route path="/eventos/:id" element={<DetalhesEvento/>}/>

            <Route path="/login" element={<Login/>}/>

            <Route path="/cadastroEvento" element={<CadastroEvento/>}/>

            <Route path="/usuarios" element={
                <ProtectedRoute role="administrador">
                    <Usuarios/>
                </ProtectedRoute>
            }/>


            <Route path="/tipoUsuario" element={<TipoUsuarioPage/>} />

            <Route path="*" element={<Navigate to="/home" replace/>}/>
        </Routes>
    )
}

export default AppRoutes;