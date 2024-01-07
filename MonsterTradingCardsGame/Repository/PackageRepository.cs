using System.Net;
using MonsterTradingCardsGame.DTOs;
using MonsterTradingCardsGame.Logic;
using Npgsql;

namespace MonsterTradingCardsGame.Repository;

public class PackageRepository {
    
    private readonly NpgsqlConnection _npg;
    private static readonly object LockCreate = new object();
    private static readonly object LockGet = new object();
    public PackageRepository(NpgsqlConnection npg) {
        _npg = npg;
    }
    
    public void CreatePackage(List<CardDTO> package) {
        lock (LockCreate) {
            var getMaxIdCommand = new NpgsqlCommand("SELECT MAX(packageid) AS max FROM package", _npg);
            using var reader = getMaxIdCommand.ExecuteReader();
            int maxPackageId;
            if (reader.Read()) {
                maxPackageId = reader.IsDBNull(reader.GetOrdinal("max")) ? 0 : reader.GetInt32(reader.GetOrdinal("max"));
                reader.Close();
            } else {
                throw new ProcessException(HttpStatusCode.InternalServerError, "");
            }
            
            foreach (var c in package) {
                var insertCommand = new NpgsqlCommand("INSERT INTO package (cardid, packageid) VALUES (@cardid, @packageid)", _npg);
                insertCommand.Parameters.AddWithValue("cardid", c.Id);
                insertCommand.Parameters.AddWithValue("packageid", maxPackageId + 1);

                insertCommand.Prepare();
                if (insertCommand.ExecuteNonQuery() != 1)
                    throw new ProcessException(HttpStatusCode.InternalServerError, "");
            }
        }
    }
    
    public PackageDTO GetPackage() {
        
        int minPackageId;
        List<CardDTO> package = new List<CardDTO>();
        lock (LockGet) {
            var getMinIdCommand = new NpgsqlCommand("SELECT MIN(packageid) AS min FROM package WHERE fk_u_id IS NULL", _npg);
            using var reader = getMinIdCommand.ExecuteReader();
            if (reader.Read() && !reader.IsDBNull(reader.GetOrdinal("min"))) {
                minPackageId = reader.GetInt32(reader.GetOrdinal("min"));
                reader.Close();
            }
            else {
                throw new ProcessException(HttpStatusCode.NotFound, "No Package available to buy\n");
            }

            using var cmd = new NpgsqlCommand("SELECT * FROM cards JOIN package p on cards.id = p.cardid WHERE packageid = @packageid", _npg);
            cmd.Parameters.AddWithValue("packageid", minPackageId);
            cmd.Prepare();
            using var reader1 = cmd.ExecuteReader();
            while (reader1.Read()) {
                package.Add(new CardDTO() {
                        Name = reader1.GetString(reader1.GetOrdinal("name")),
                        Damage = reader.GetFloat(reader1.GetOrdinal("damage")),
                        Id = reader1.GetString(reader1.GetOrdinal("id"))
                    }
                );
            }
        }
        
        return new PackageDTO() {
            Package = package,
            PackageId = minPackageId
        };
    }
    
    /*public void DeletePackage(int minPackageId) {

        var deleteCommand = new NpgsqlCommand("DELETE FROM package WHERE packageid = @packageid", _npg);
        deleteCommand.Parameters.AddWithValue("packageid", minPackageId);
        deleteCommand.Prepare();
        if(deleteCommand.ExecuteNonQuery() < 1)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
    }*/
    
    public void UpdatePackage(int minPackageId, string username) {
        
        var updateCommand = new NpgsqlCommand("UPDATE package SET fk_u_id = (SELECT u_id FROM users WHERE username = @username) WHERE packageid = @packageid", _npg);
        updateCommand.Parameters.AddWithValue("username", username);
        updateCommand.Parameters.AddWithValue("packageid", minPackageId);
        updateCommand.Prepare();
        if(updateCommand.ExecuteNonQuery() < 1)
            throw new ProcessException(HttpStatusCode.InternalServerError, "");
    }
}