# **SharpWrap2534 - The Ultimate J2534 Wrapper Suite**

### **What is SharpWrap2534?**
SharpWrap2534 is an approach at trying to build and deploy and easily consumable, user friendly, J2534 API wrapper in C# (along with some nifty extension packages that add support for more features). While there's a good number of these out there, SharpWrap stands out from the rest for a number of reasons. Some of the key features of SharpWrap are:
-  Both Version 04.04 and Version 05.00 APIs are supported completely!
-  All J2534 calls are setup in the background leaving the user with minimal configuration needed to consume this library
-  Setting up a new instance of a J2534 device can be done in **ONE LINE OF CODE**. 
   -  All configuration of the J2534 Device and DLL objects are done when a new SharpSession is started. 
   -  When a SharpSession is built, a DLL is built based on the name provided and then the next possible device is automatically loaded up as well.
- Any exceptions thrown inside the J2534 instances are always returned out to the user for easy debugging.
-  A total of (2) two device instances can be generated at one time. By forcing this design pattern, it's nearly impossible to try and build instances of devices which are impossible. 
   -  Like the device instances, only a set number of J2534 Channels can exist at a time to prevent losing track of them. 
   -  If Logical channels are needed, then they are built as well.
-  Supports the hidden DrewTech API for device locating and initalization processes 
   -   These methods include the PTGetNextDevice() PTGetNextCarDAQ() PTScanForDevice() and more.
   -   Each of these methods are mapped using their pointer delegates for unmanaged access and when called, are passed through an API Marshall to convert them into managed types.
-   For more information on each of the different packages inside this repository, go into each directory according to the name of the package. There's a README for each of them inside there.

---

### **Current Extensions**
- As of now, there's a collection of 4/5 extensions ready for the SharpWrapper package.
- They're all located in the Extensions folder of this repository and each have their own requirements. 
- As of 1/31/2023, we've got the SharpAudoId, SharpExpressions, SharpPipes, and SharpSimulator packages ready to run. Just grab them from NuGet like you'd pull in the SharpWrapper package.

---

### **Development Setup**
- NOTE: As of 2/17/2023 - I've closed down the readonly access for anyone to use. If you want to use these packages, please contact zack.walsh@meatinc.autos for an API key, and someone will walk you through getting into this package repository. This decision was made after realizing that while the key was readonly and on a dedicated bot account, it's not the best idea to leave API keys exposed. And since making these projects public, it was only logical to remove the keys from here.
- If you're looking to help develop this project, you'll need to add the NuGet server for the MEAT Inc workspace into your nuget configuration. 
- To do so, navigate to your AppData\Roaming folder (You can do this by opening windows explorer and clicking the top path bar and typing %appdata%)
- Now find the folder named NuGet and open the file named NuGet.config
- Inside this file, under packageSources, you need to add a new source. Insert the following line into here 
     ```XML 
      <add key="MEAT-Inc" value="https://nuget.pkg.github.com/MEAT-Inc/index.json/" protocolVersion="3" />
    ```
- Once added in, scroll down to packageSourceCredentials (if it's not there, just make a new section for it)
- Inside this section, put the following block of code into it.
   ```XML
    <MEAT-Inc>
       <add key="Username" value="meatincreporting" />
       <add key="ClearTextPassword" value="{INSERT_API_KEY_HERE}" />
    </MEAT-Inc>
    ```
 - Once added in, save this file and close it out. 
 - Your NuGet.config should look something like this. This will allow you to access the packages inside the MEAT Inc repo/workspaces to be able to build the solution.
    ```XML
      <?xml version="1.0" encoding="utf-8"?>
          <configuration>
              <packageSources>
                  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
                  <add key="MEAT-Inc" value="https://nuget.pkg.github.com/MEAT-Inc/index.json/" protocolVersion="3" />
              </packageSources>
              <packageSourceCredentials>
                  <MEAT-Inc>
                      <add key="Username" value="meatincreporting" />
                      <add key="ClearTextPassword" value="{INSERT_API_KEY_HERE}" />
                  </MEAT-Inc>
              </packageSourceCredentials>
              <packageRestore>
                  <add key="enabled" value="True" />
                  <add key="automatic" value="True" />
              </packageRestore>
              <bindingRedirects>
                  <add key="skip" value="False" />
              </bindingRedirects>
              <packageManagement>
                  <add key="format" value="1" />
                  <add key="disabled" value="True" />
              </packageManagement>
          </configuration> 

---

### Questions, Comments, Concerns? 
- I don't wanna hear it...
- But feel free to send an email to zack.walsh@meatinc.autos. He might feel like being generous sometimes...
- Or if you're feeling like a good little nerd, make an issue on this repo's project and I'll take a peek at it.

