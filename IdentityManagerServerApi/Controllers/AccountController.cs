    using IdentityManagerServerApi.Data;
    using IdentityManagerServerApi.Models;
    using Microsoft.AspNetCore.Mvc;
    using SharedClassLibrary.Contracts;
    using SharedClassLibrary.DTOs;
    namespace IdentityManagerServerApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class AccountController(IUserAccount userAccount) : ControllerBase
        {


             AppDbContext db = new AppDbContext();

            [HttpPost("register")]
            public async Task<IActionResult> Register(UserDTO userDTO)
            {
                var response = await userAccount.CreateAccount(userDTO);


                if (userDTO.IsDoctor == true)
                {

                var newDoctor = new Doctor
                {
                    Name = "First",
                    Description = "Description ",
                    Specialization = "Specialization"
                };

                db.Add(newDoctor);
                db.SaveChanges();

                }
   
               else if (userDTO.IsStudent)
                {


                    var newStudent = new Student
                    {
                        Name = "First Student",
                        Description = "Description",
                        UnvirestyName = "UnvirestyName"

                    };

                    db.Add(newStudent);
                    db.SaveChanges();

                }


                else if (userDTO.IsPatient)
                {


                    var newPatient = new Patient
                    {
                        Name = "First Patient",
                        Description = "Description",
                        HealthCondition = "HealthCondition"
                    };

                    db.Add(newPatient);
                    db.SaveChanges();

                }




                return Ok(response);
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginDTO loginDTO)
            {
                var response = await userAccount.LoginAccount(loginDTO);
                return Ok(response);
            }
        }
    }
