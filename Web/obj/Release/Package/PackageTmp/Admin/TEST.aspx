<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TEST.aspx.cs" Inherits="Web.Admin.TEST" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312">
<title>�½���ҳ 1</title>
</head>

<body>   
    <form runat="server">

<div>  
      
        <fieldset>  
<legend>����</legend>  
            <asp:FileUpload ID="FileUpload1" runat="server" />  
            <asp:Button ID="Button1" runat="server"  
                Text="�����ύ����" OnClick="Button1_Click" />  
  
        </fieldset>  
   <br />  
      
                <fieldset>  
<legend>����</legend>  
  
                    <asp:FileUpload ID="FileUpload2" runat="server" />  
  
            <asp:Button ID="Button2" runat="server"  
                Text="�����ύ����" OnClick="Button2_Click"  />  
  
                </fieldset>  
    </div>  
        </form>
</body>

</HTML>
