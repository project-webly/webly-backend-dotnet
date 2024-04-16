namespace Webly.Configurations;

public record AppConfig(
	string ValidIssuer,
	string ValidAudience,
	string Secret)
{
	public static AppConfig ForProduct()
	{
		return new AppConfig(
			ValidIssuer: Environment.GetEnvironmentVariable(nameof(ValidIssuer)),
			ValidAudience: Environment.GetEnvironmentVariable(nameof(ValidAudience)),
			Secret: Environment.GetEnvironmentVariable(nameof(Secret)));
	}

	public static AppConfig ForDebug()
	{
		return new AppConfig(
			ValidIssuer: "localhost",
			ValidAudience: "localhost",
			Secret: "ABABABABABABAADFSDFSDFDSFSDFDSFSDFDSFSDFSDFSDFSDFSDF");
	}
}