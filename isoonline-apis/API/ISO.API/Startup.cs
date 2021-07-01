using ISO.API.Middleware;
using ISO.API.Models;
using ISO.API.Service;
using ISO.API.Service.Implement;
using ISO.API.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IO;
using System.Text;

namespace ISO.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration/*, Microsoft.AspNetCore.Hosting.IHostingEnvironment env*/)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            //var signingKey = Convert.FromBase64String(Configuration["Jwt:SigningSecret"]);
            //            var signingKey = Encoding.UTF8.GetBytes(Configuration["Jwt:SigningSecret"]);
            //            options.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuer = false,
            //                ValidateAudience = false,
            //                ValidateIssuerSigningKey = true,
            //                IssuerSigningKey = new SymmetricSecurityKey(signingKey)
            //            };
            //        });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "TTXVN API",
                    Version = "v1",
                    Description = "TTXVN API",
                });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".xml");
                if (File.Exists(filePath))
                {
                    options.IncludeXmlComments(filePath);
                }
                options.EnableAnnotations();
            });

            services.AddControllers();

            services.AddSingleton<IHumanDepartmentService, HumanDepartmentService>();
            services.AddSingleton<IDepartmentTitleService, DepartmentTitleService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<IProblemTypeService, ProblemTypeService>();
            services.AddSingleton<IProblemCriticalLevelService, ProblemCriticalLevelService>();
            services.AddSingleton<IPositionService, PositionService>();
            services.AddSingleton<IManagementLevelService, ManagementLevelService>();
            services.AddSingleton<IDepartmentTitleMenuService, DepartmentTitleMenuService>();
            services.AddSingleton<IProblemEventService, ProblemEventService>();
            services.AddSingleton<IProblemEmergencyService, ProblemEmergencyService>();
            services.AddSingleton<IProblemStatusService, ProblemStatusService>();
            services.AddSingleton<IProblemResidentAgencyGroupService, ProblemResidentAgencyGroupService>();
            services.AddSingleton<IProblemResidentAgencyService, ProblemResidentAgencyService>();
            services.AddSingleton<ITitleService, TitleService>();
            services.AddSingleton<IBusinessModuleService, BusinessModuleService>();
            services.AddSingleton<IProblemEventUserService, ProblemEventUserService>();
            services.AddSingleton<IDepartmentTitleUserService, DepartmentTitleUserService>();
            services.AddSingleton<IHumanEmployeeService, HumanEmployeeService>();
            services.AddSingleton<IProblemGroupService, ProblemGroupService>();
            services.AddSingleton<IProblemEventReportService, ProblemEventReportSerivce>();
            services.AddSingleton<IProblemEventDepService, ProblemEventDepService>();
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<IProblemEventRequestDepService, ProblemEventRequestDepService>();
            services.AddSingleton<IShiftHistoryService, ShiftHistoryService>();
            services.AddSingleton<IProfileHumanPermissionService, ProfileHumanPermissionService>();
            //danh muc ho so ly lich
            services.AddSingleton<ICityService, CityService>();
            services.AddSingleton<IDistrictService, DistrictService>();
            services.AddSingleton<ICommuneService, CommuneService>();
            services.AddSingleton<INationalityService, NationalityService>();
            services.AddSingleton<IEthnicService, EthnicService>();
            services.AddSingleton<IPositionMilitaryService, PositionMilitaryService>();
            services.AddSingleton<IPositionPartyService, PositionPartyService>();
            services.AddSingleton<IBloodTypeService, BloodTypeService>();
            services.AddSingleton<IAgeService, AgeService>();
            services.AddSingleton<IYouthPositionService, YouthPositionService>();
            services.AddSingleton<IHumanProfileCuriculmViateService, HumanProfileCuriculmViateService>();
            services.AddSingleton<IReligionService, ReligionService>();
            services.AddSingleton<IGovermentManagementService, GovermentManagementService>();
            services.AddSingleton<IPoliticalTheoryService, PoliticalTheoryService>();
            //danh muc ho so qua trinh dao tao
            services.AddSingleton<IEducationTypeService, EducationTypeService>();
            services.AddSingleton<IEducationResultService, EducationResultService>();
            services.AddSingleton<IEducationResultService, EducationResultService>();
            services.AddSingleton<IHumanProfileTrainingService, HumanProfileTrainingService>();
            //danh muc ho so van bang
            services.AddSingleton<IEducationFieldService, EducationFieldService>();
            services.AddSingleton<IEducationOrgService, EducationOrgService>();
            services.AddSingleton<IEducationLevelService, EducationLevelService>();
            services.AddSingleton<ICertificateLevelService, CertificateLevelService>();
            services.AddSingleton<IHumanProfileDiplomaService, HumanProfileDiplomaService>();
            //danh muc ho so chung chi
            services.AddSingleton<ICertificateTypeService, CertificateTypeService>();
            services.AddSingleton<IHumanProfileCertificateService, HumanProfileCertificateService>();
            //danh muc ho so khen thuong
            services.AddSingleton<IAwardCategoryService, AwardCategoryService>();
            services.AddSingleton<IHumanProfileRewardService, HumanProfileRewardService>();
            //danh muc hinh thuc ki luat
            services.AddSingleton<IDisciplineCategoryService, DisciplineCategoryService>();
            services.AddSingleton<IHumanProfileDisciplineService, HumanProfileDisciplineService>();
            //danh muc ho so hop dong lao dong
            services.AddSingleton<IContractTypeService, ContractTypeService>();
            services.AddSingleton<IContractStatusService, ContractStatusService>();
            services.AddSingleton<IHumanProfileContractService, HumanProfileContractService>();
            //danh muc ho so quan he gia dinh
            services.AddSingleton<IFamilyRelationshipService, FamilyRelationshipService>();
            services.AddSingleton<IHumanProfileRelationshipService, HumanProfileRelationshipService>();
            //ho so luong
            services.AddSingleton<IHumanProfileSalaryService, HumanProfileSalaryService>();
            //ho so dinh kem
            services.AddSingleton<IHumanProfileAttachmentService, HumanProfileAttachmentService>();
            //ho so danh gia
            services.AddSingleton<IHumanProfileAssessesService, HumanProfileAssessesService>();
            //ho so bao hiem
            services.AddSingleton<IHumanProfileInsuranceService, HumanProfileInsurancesService>();
            //ho so thu viec
            services.AddSingleton<IHumanProfileWorkTrialService, HumanProfileWorkTrialService>();
            //Dashboard nhan su
            services.AddSingleton<IHumanDashboardService, HumanDashboardService>();
            services.AddSingleton<IHumanProfileWorkExperienceService, HumanProfileWorkExperienceService>();

            // tai lieu
            services.AddSingleton<IDocumentService, DocumentService>();

            // chấm công
            services.AddSingleton<ITimekeeperService, TimekeeperService>();

            //thong ke nhan su
            services.AddSingleton<IStatisticEmpoyeeService, StatisticEmpoyeeService>();
            //Thư mục tài liệu
            services.AddSingleton<IDocumentFolderService, DocumentFolderService>();
            services.AddSingleton<IDocumentDepartmentService, DocumentDepartmentService>();
            services.AddSingleton<IDocumentEmployeeService, DocumentEmployeeService>();

            //Chấm công
            services.AddSingleton<IWorkOutService, WorkOutService>();
            services.AddSingleton<ITimeKeepingService, TimeKeepingService>();
            services.AddSingleton<ITimekeeperService, TimekeeperService>();
            services.AddSingleton<IEmployeeOTService, EmployeeOTService>();

            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRequestResponseLogging();

            //app.UseMiddleware<RequestResponseLoggingMiddleware>();
            //app.UseSerilogRequestLogging(opts => opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest);
        }
    }
}