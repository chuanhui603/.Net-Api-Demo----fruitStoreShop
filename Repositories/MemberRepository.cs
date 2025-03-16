namespace 水水水果API.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly string _tableName = "member";
        public MemberRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IEnumerable<Member> GetMembers()
        {
            return _dbConnection.GetAll<Member>(_tableName);
        }

        public Member GetMemberById(Guid id)
        {
            return _dbConnection.GetById<Member>(_tableName, id);
        }

        public void CreateMember(Member member)
        {
            _dbConnection.Insert(_tableName, member);
        }

        public void UpdateMember(Member member)
        {
            _dbConnection.Update(_tableName, member);
        }

        public void DeleteMember(Guid id)
        {
            _dbConnection.Delete(_tableName, id);
        }

        public Member GetMemberByLogin(MemberDTO member)
        {
            var sql = "SELECT login_role,email,id FROM member WHERE email = @Email";
            return _dbConnection.QuerySingle<Member>(sql, new { member.Email, member.Password,member.Id });
        }
    }
}