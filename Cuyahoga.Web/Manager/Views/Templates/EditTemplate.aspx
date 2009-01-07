﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Manager/Views/Shared/Admin.Master" AutoEventWireup="true" CodeBehind="EditTemplate.aspx.cs" Inherits="Cuyahoga.Web.Manager.Views.Templates.EditTemplate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphTasks" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="server">
	<% using (Html.BeginForm("Update", "Templates", new { id = ViewData.Model.Id }, FormMethod.Post, new { id = "templateform" })) { %>
		<fieldset>
			<legend><%=GlobalResources.GeneralLabel%></legend>
			<ol>
				<li>
					<label for="Name"><%=GlobalResources.NameLabel%></label>
					<%=Html.TextBox("Name", ViewData.Model.Name, new { style = "width:300px" })%>
				</li>
				<li>
					<label for="BasePath"><%=GlobalResources.BasePathLabel %></label>
					<%= ViewData.Model.BasePath %>
				</li>
				<li>
					<label for="TemplateControl"><%=GlobalResources.TemplateControlLabel %></label>
					<%=Html.DropDownList("TemplateControl", ViewData["TemplateControls"] as SelectList)%>
				</li>
				<li>
					<label for="Css"><%=GlobalResources.CssLabel %></label>
					<%=Html.DropDownList("Css", ViewData["CssFiles"] as SelectList)%>
				</li>
			</ol>
		</fieldset>
			
		<input type="submit" value="<%= GlobalResources.SaveButtonLabel %>" />
		<%= GlobalResources.Or %>
		<%= Html.ActionLink(GlobalResources.CancelLabel, "Index") %>
	<% } %>
</asp:Content>