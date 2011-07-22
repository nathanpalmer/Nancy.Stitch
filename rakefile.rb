require 'rubygems'
require 'albacore'
require 'rake/clean'

NANCY_STITCH_VERSION = "0.1.0"
OUTPUT = "build"
CONFIGURATION = 'Release'
SHARED_ASSEMBLY_INFO = 'src/SharedAssemblyInfo.cs'
SOLUTION_FILE = 'src/Nancy.Stitch.sln'

Albacore.configure do |config|
	config.log_level = :verbose
	config.msbuild.use :net4
end

desc "Compiles solution and runs unit tests"
task :default => [:clean, :version, :compile, :publish, :package]

#Add the folders that should be cleaned as part of the clean task
CLEAN.include(OUTPUT)
CLEAN.include(FileList["src/**/#{CONFIGURATION}"])

desc "Update shared assemblyinfo file for the build"
assemblyinfo :version => [:clean] do |asm|
	asm.version = NANCY_STITCH_VERSION
	asm.company_name = "Nancy"
	asm.product_name = "Nancy"
	asm.title = "Nancy.Stitch"
	asm.description = "Stitch pipeline extension for Nancy (A Sinatra inspired web framework for the .NET platform)"
	asm.copyright = "Copyright (C) Nathan Palmer"
	asm.output_file = SHARED_ASSEMBLY_INFO
end

desc "Compile solution file"
msbuild :compile => [:version] do |msb|
	msb.properties :configuration => CONFIGURATION
	msb.targets :Clean, :Build
	msb.solution = SOLUTION_FILE
end

desc "Gathers output files and copies them to the output folder"
task :publish => [:compile] do
	Dir.mkdir(OUTPUT)
	Dir.mkdir("#{OUTPUT}/binaries")

	FileUtils.cp_r FileList["src/**/#{CONFIGURATION}/Nancy.Stitch.dll", "src/**/#{CONFIGURATION}/Nancy.Stitch.pdb"].exclude(/obj\//).exclude(/.Tests/), "#{OUTPUT}/binaries"
end

desc "Zips up the built binaries for easy distribution"
zip :package => [:publish] do |zip|
	Dir.mkdir("#{OUTPUT}/packages")

	zip.directories_to_zip "#{OUTPUT}/binaries"
	zip.output_file = "Nancy.Stitch-Latest.zip"
	zip.output_path = "#{OUTPUT}/packages"
end
