<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ShutDownDetails.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: #E1E1E1">
    <form id="form1" runat="server">
      <div align="center"  >
         <div style="border-style: outset; border-width: medium; width:900px; margin: 0px auto 0px auto; text-align:center" align="center">
            <div style="background-color: #FFFFFF">
                 <table style="width:900px">
                    <tr>
                         <td align="left" width="70%">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/LogoHP.PNG" />                            
                        </td>
                       
                    </tr>
                </table>
            </div>
            <div style="border-width: thin; border-color: #FF0000; width:100%; font-weight: bold; font-family: 'Times New Roman', Times, serif; font-size: xx-large; font-style: italic; background-color:#0000FF; color: #FFFFFF; border-top-style: solid; border-bottom-style: solid;" 
                align="center">
                <table align="center" width="100%">
                    <tr>
                        <td align="center">
                            Shut Down Details
                        </td>
                    </tr>
                </table>
            </div>
             <div style="width:100%" align="center">
             <table width="70%">
                <tr>
                    <td width="40%">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/MR Photo.png" 
                            Height="186px" Width="360px" />
                    </td>
                    <td width="20%">
                     <table width="100%" bgcolor="Silver" frame="hsides" style="border-radius: 10px">
                            <tr>
                                <td width="100%" align="center" bgcolor="#F95252" 
                                    style="border-style: solid; border-width: thin; border-radius: 10px; ">                               
                                    <asp:Label ID="lblogin" runat="server" Text="Log In" Font-Bold="True" Font-Names="sans-serif" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td width="45%" align="left">
                                                <asp:Label ID="lbempno" runat="server" Text="Login ID" ForeColor="#6600FF" 
                                                    Font-Bold="False"></asp:Label>    
                                            </td>
                                            <td width="45%">
                                                <asp:TextBox ID="txtuserid" runat="server" MaxLength="8" Width="130px" 
                                                    BackColor="#FEE9ED"></asp:TextBox>    
                                            </td>
                                            <td width="10%">
                                                <asp:RequiredFieldValidator ID="rfvfouempno" runat="server" ControlToValidate="txtuserid" ErrorMessage="*" Font-Names="Times New Roman" ForeColor="Red"></asp:RequiredFieldValidator>    
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                             <tr>
                                <td width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td width="40%" align="left">
                                                <asp:Label ID="lbpassword" runat="server" Text="Password" ForeColor="#6600FF"></asp:Label>    
                                            </td>
                                            <td width="50%">
                                                <asp:TextBox ID="txtpassword" runat="server" TextMode="Password" Width="130px" 
                                                    BackColor="#FEE9ED"></asp:TextBox>    
                                            </td>
                                            <td width="10%">
                                                <asp:RequiredFieldValidator ID="rfvforpassword" runat="server" ControlToValidate="txtpassword" ErrorMessage="*" Font-Names="Times New Roman" ForeColor="Red"></asp:RequiredFieldValidator>    
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        <tr>
                            <td width="100%">
                                <asp:Label ID="lbstatus" runat="server" Font-Names="Times New Roman"  
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" align="center">
                                            <asp:Button ID="btnlogin" runat="server" Text="LogIn" onclick="btnlogin_Click" 
                                                Font-Bold="True"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="color: #000080; font-style: italic">
                                Please Login With 
                                your 8 Digit Employee ID 
                                and ADS Password
                            </td>
                        </tr>
                    </table>  
                    </td>
                    <td width="40%">
                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/VR Photo.png" 
                            Height="184px" Width="299px" />
                    </td>
                </tr>
             </table>
            
        </div>
       </div>
      </div>
    </form>
</body>

</html>
