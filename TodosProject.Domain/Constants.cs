using System;

namespace TodosProject.Domain
{
    public class Constants
    {
        public const int SaltSize = 16;
        public const int HashSize = 20;

        public const string ExceptionRequestAPI = "Erro ao efetuar request da Api externa: {0}";
        public const string ExceptionExcel = "Erro ao gerar o excel solicitado";

        public const string ErrorInAdd = "Ocorreu um erro ao adicionar um novo registro. Entre em contato com o Administrador";
        public const string ErrorInUpdate = "Ocorreu um erro ao adicionar um novo registro. Entre em contato com o Administrador";
        public const string ErrorInDelete = "Ocorreu um erro ao deletar o registro. Entre em contato com o Administrador";
        public const string ErrorInResearch = "Ocorreu um erro ao adicionar um novo registro. Entre em contato com o Administrador";
        public const string ErrorInLogin = "As autenticações fornecidas são invalidas. tente novamente!";
        public const string ErrorInChangePassword = "Ocorreu um erro ao efetuar a troca de senha";
        public const string ErrorInResetPassword = "Ocorreu um erro ao efetuar o reset de senha";
        public const string SuccessInLogin = "A autenticação foi efetuada com sucesso";
        public const string SuccessInAdd = "O registro foi adicionado com sucesso";
        public const string SuccessInUpdate = "O registro foi atualizado com sucesso";
        public const string SuccessInDelete = "O registro foi excluído com sucesso";
        public const string SuccessInResearch = "O registro foi localizado com sucesso";
        public const string SuccessInChangePassword = "A troca de senha foi efetuada com sucesso";
        public const string SuccessInResetPassword = "O reset de senha foi efetuado com sucesso";
    }
}
