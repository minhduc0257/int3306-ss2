## Backend của [P4L](https://github.com/huetrantt21020018/P4L/)
##### Yêu cầu : .NET 7 SDK, database MySQL

- Clone repository này về và thực hiện các lệnh dưới đây ở thư mục gốc.
- Set các biến môi trường sau. `.env` được hỗ trợ.  
  - `MARIADB_CONNECTION_STRING`=`Server=yourserver;Port=your_port;User=your_user;Password=your_user;Database=your_db`
  - `JWT_KEY`=`khóa cho JWT`
  - `JWT_ISSUER`=`bên cấp token JWT`
  - `JWT_AUDIENCE`=`trường audience trong JWT`
  - `STATIC_PATH`=`đường dẫn tuyệt đối đến frontend; index.html sẽ được serve từ đây`
  - `DATA_PATH`=`đường dẫn tuyệt đối đến folder chứa các tập tin được tải lên`
- Chạy `dotnet run --project int3306.Api`
  - Yêu cầu port 5000 còn trống. 